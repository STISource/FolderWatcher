using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace FolderWatcher.Models
{
    [DataContract]
    public class FolderWatcherSettings
    {
        [DataMember]
        ObservableCollection<FolderDetails> Folders { get; set; }
    }
}
