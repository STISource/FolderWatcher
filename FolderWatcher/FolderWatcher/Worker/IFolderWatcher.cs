using FolderWatcher.Models;
using System;
using System.Collections.Generic;

namespace FolderWatcher.Worker
{
    public interface IFolderWatcher
    {
        FolderDetails WatchedFolder { get; }

        void Watch(FolderDetails folder);

        void StopWatching();

        event FolderContentChangedEventHandler InputArchiveChanged;
    }

    public class FolderContentChangedEventArgs : EventArgs
    {
        public IEnumerable<FileDetails> CreatedFiles { get; set; }
    }

    public delegate void FolderContentChangedEventHandler(object sender, FolderContentChangedEventArgs args);
}
