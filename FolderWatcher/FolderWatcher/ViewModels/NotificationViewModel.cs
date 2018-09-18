using FolderWatcher.Helpers;
using FolderWatcher.Models;
using System;
using System.Windows.Input;

namespace FolderWatcher.ViewModels
{
    public class NotificationViewModel
    {
        public event EventHandler OnDismiss;

        public NewFileNotification Notification { get; set; }

        public ICommand Dismiss { get; set; }

        public NotificationViewModel(NewFileNotification notification)
        {
            this.Notification = notification;
            this.Dismiss = new SimpleCommand(() => true, this.DismissNotification);
        }

        private void DismissNotification()
        {
            this.OnDismiss?.Invoke(this, EventArgs.Empty);
        }
    }
}
