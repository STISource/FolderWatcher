using System;
using System.Runtime.Serialization;

namespace FolderWatcher.Models
{
    [DataContract]
    public class FileDetails
    {
        [DataMember]
        public Guid FolderId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public DateTime ChangeDate { get; set; }
    }
}
