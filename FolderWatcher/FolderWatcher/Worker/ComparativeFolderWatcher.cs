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

            var oldSnapshot = this.snapshotService.ReadSnapshot(this.WatchedFolder.FolderId);
            var currentSnapshot = this.snapshotService.CreateSnapshot(this.WatchedFolder);
            var newFiles = FolderContentComparer.GetNewFiles(currentSnapshot, oldSnapshot);
            var deletedFiles = FolderContentComparer.GetDeletedFiles(currentSnapshot, oldSnapshot);

            if ((newFiles != null && newFiles.Any())
                || (deletedFiles != null && deletedFiles.Any()))
            {
                this.RaiseFolderContentChange(newFiles, deletedFiles);                
            }

            // save potential changes into snapshot            
            currentSnapshot.Id = oldSnapshot.Id;
            this.snapshotService.UpdateSnapshot(currentSnapshot);
        }

        private void RaiseFolderContentChange(IEnumerable<FileDetails> createdFiles, IEnumerable<FileDetails> deletedFiles)
        {
            this.FolderContentChanged?.Invoke(this, new FolderContentChangedEventArgs
            {
                CreatedFiles = createdFiles,
                DeletedFiles = deletedFiles
            });
        }
    }
}
