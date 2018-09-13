using System;
using FolderWatcher.Models;

namespace FolderWatcher.Services
{
    public interface IFolderContentSnapshotService
    {
        void UpdateSnapshot(FolderContentSnapshot snapshot);

        FolderContentSnapshot ReadSnapshot(Guid folderId);
    }
}
