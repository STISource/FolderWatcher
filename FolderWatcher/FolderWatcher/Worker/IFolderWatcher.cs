using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FolderWatcher.Models;

namespace FolderWatcher.Worker
{
    public interface IFolderWatcher
    {
        FolderDetails WatchedFolder { get; }

        void Watch(FolderDetails folder);

        void StopWatching();

        event FolderContentChangedEventHandler FolderContentChanged;
    }

    public class FolderContentChangedEventArgs : EventArgs
    {
        public IEnumerable<FileDetails> CreatedFiles { get; set; }
    }

    public delegate void FolderContentChangedEventHandler(object sender, FolderContentChangedEventArgs args);
}
