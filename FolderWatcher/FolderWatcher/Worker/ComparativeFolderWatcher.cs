using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FolderWatcher.Models;
using FolderWatcher.Services;

namespace FolderWatcher.Worker
{
    public class ComparativeFolderWatcher : IFolderWatcher
    {
        private const int PollingInterval = 8000;

        private readonly IFolderContentSnapshotService snapshotService;

        private Timer timer;

        private bool watching = false;

        public FolderDetails WatchedFolder { get; set; }

        public event FolderContentChangedEventHandler FolderContentChanged;

        public ComparativeFolderWatcher(IFolderContentSnapshotService snapshotService)
        {
            this.snapshotService = snapshotService;            
        }

        public void Watch(FolderDetails folder)
        {
            this.WatchedFolder = folder;
            this.watching = true;

            this.timer = new Timer(this.CheckForFolderChanges, null, 1000, PollingInterval);
        }

        public void StopWatching()
        {
            this.watching = false;
            this.timer.Dispose();
        }

        private void CheckForFolderChanges(object state)
        {
            if(!this.watching)
            {
                return;
            }

            var snapshot = this.snapshotService.ReadSnapshot(this.WatchedFolder.FolderId);
            var newFiles = FolderContentComparer.GetNewFiles(this.WatchedFolder, snapshot);

            if (newFiles != null && newFiles.Any())
            {
                this.RaiseFolderContentChange(newFiles);                
            }

            // save potential changes into snapshot
            var newSnapshot = this.snapshotService.CreateSnapshot(this.WatchedFolder);
            newSnapshot.Id = snapshot.Id;
            this.snapshotService.UpdateSnapshot(newSnapshot);
        }

        private void RaiseFolderContentChange(IEnumerable<FileDetails> createdFiles)
        {
            this.FolderContentChanged?.Invoke(this, new FolderContentChangedEventArgs
            {
                CreatedFiles = createdFiles
            });
        }
    }
}
