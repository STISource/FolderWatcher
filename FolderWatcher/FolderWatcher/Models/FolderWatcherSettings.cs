using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace FolderWatcher.Models
{
    [DataContract]
    public class FolderWatcherSettings : INotifyPropertyChanged
    {
        private bool hideTrayIconIfNoNotifications;

        private bool keepNotificationsForDeletedFiles;

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
        public bool KeepNotificationsForDeletedFiles
        {
            get
            {
                return this.keepNotificationsForDeletedFiles;
            }

            set
            {
                if (this.keepNotificationsForDeletedFiles != value)
                {
                    this.keepNotificationsForDeletedFiles = value;
                    this.RaisePropertyChanged("KeepNotificationsForDeletedFiles");
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
