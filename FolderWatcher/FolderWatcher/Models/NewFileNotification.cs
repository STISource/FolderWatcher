using System;

namespace FolderWatcher.Models
{
    public class NewFileNotification
    {
        public int Id { get; set; }

        public DateTime ChangeDate { get; set; }

        public string Folder { get; set; }

        public string File { get; set; }
    }
}
