using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace FolderWatcher.Models
{
    [DataContract]
    public class FolderDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Guid folderId;

        private string folderName;

        [DataMember]
        public Guid FolderId
        {
            get
            {
                return this.folderId;
            }

            set
            {
                if (this.folderId != value)
                {
                    this.folderId = value;
                    this.NotifyOfPropertyChanges("FolderId");
                }
            }
        }

        [DataMember]
        public string FolderName
        {
            get
            {
                return this.folderName;
            }

            set
            {
                if (this.folderName != value)
                {
                    this.folderName = value;
                    this.NotifyOfPropertyChanges("FolderName");
                }
            }
        }

        private void NotifyOfPropertyChanges(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
