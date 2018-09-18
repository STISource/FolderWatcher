using System.Collections.Generic;
using System.Linq;
using FolderWatcher.Models;
using LiteDB;

namespace FolderWatcher.Services
{
    public class NoSqlNotificationHistoryService : INotificationHistoryService
    {
        public const string DatabaseName = @"NotificationHistory.db";
        public const string CollectionName = @"HistoryEntries";

        public void Create(NewFileNotification notification)
        {
            using (var database = new LiteDatabase(DatabaseName))
            {
                var historyEntries = database.GetCollection<NewFileNotification>(CollectionName);

                historyEntries.Insert(notification);
            }
        }

        public void Delete(NewFileNotification notification)
        {
            using (var database = new LiteDatabase(DatabaseName))
            {
                var historyEntries = database.GetCollection<NewFileNotification>(CollectionName);

                historyEntries.Delete(notification.Id);
            }
        }

        public IEnumerable<NewFileNotification> ReadAll()
        {
            using (var database = new LiteDatabase(DatabaseName))
            {
                return database.GetCollection<NewFileNotification>(CollectionName).FindAll().OrderBy(x => x.ChangeDate);                
            }
        }
    }
}
