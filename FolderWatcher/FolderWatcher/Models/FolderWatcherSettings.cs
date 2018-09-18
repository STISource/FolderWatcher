using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace FolderWatcher.Models
{
    [DataContract]
    public class FolderWatcherSettings
    {
        [DataMember]
        public ObservableCollection<FolderDetails> Folders { get; set; }
    }
}
