using FolderWatcher.Models;
using System.Collections.Generic;

namespace FolderWatcher.Services
{
    public interface INotificationHistoryService
    {
        IEnumerable<NewFileNotification> ReadAll();

        void Create(NewFileNotification notification);

        void Delete(NewFileNotification notification);
    }
}
