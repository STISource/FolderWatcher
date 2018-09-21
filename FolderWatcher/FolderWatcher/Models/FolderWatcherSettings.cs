using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace FolderWatcher.Models
{
    [DataContract]
    public class FolderWatcherSettings : INotifyPropertyChanged
    {
        private bool hideTrayIconIfNoNotifications;

        private ObservableCollection<FolderDetails> folders;

        [DataMember]
        public bool HideTrayIconIfNoNotifications
        {
            get
            {
                return this.hideTrayIconIfNoNotifications;
            }

            set
            {
                if(this.hideTrayIconIfNoNotifications != value)
                {
                    this.hideTrayIconIfNoNotifications = value;
                    this.RaisePropertyChanged("HideTrayIconIfNoNotifications");
                }
            }
        }

        [DataMember]
        public ObservableCollection<FolderDetails> Folders
        {
            get
            {
                return this.folders;
            }

            set
            {
                if(this.folders != value)
                {
                    this.folders = value;
                    this.RaisePropertyChanged("Folders");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
