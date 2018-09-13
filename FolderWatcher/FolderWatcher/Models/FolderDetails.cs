using System;
using System.Runtime.Serialization;

namespace FolderWatcher.Models
{
    [DataContract]
    public class FolderDetails
    {
        [DataMember]
        public Guid FolderId { get; set; }

        [DataMember]
        public string FolderName { get; set; }
    }
}
