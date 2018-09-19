using System;
using System.Windows.Input;
using FolderWatcher.Helpers;
using FolderWatcher.Models;
using FolderWatcher.Services;
using NLog;
using Unity;

namespace FolderWatcher.ViewModels
{
    public class NotificationViewModel
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IUnityContainer iocContainer;

        public event EventHandler OnDismiss;

        public NewFileNotification Notification { get; set; }

        public ICommand Dismiss { get; set; }

        public ICommand OpenInExplorer { get; set; }

        public NotificationViewModel(NewFileNotification notification, IUnityContainer iocContainer)
        {
            this.Notification = notification;
            this.iocContainer = iocContainer;
            this.Dismiss = new SimpleCommand(() => true, this.DismissNotification);
            this.OpenInExplorer = new SimpleCommand(() => true, this.OpenFileInExplorer);
        }

        public void DismissNotification()
        {
            logger.Info("Dismiss single notification");
            this.OnDismiss?.Invoke(this, EventArgs.Empty);
        }

        private void OpenFileInExplorer()
        {
            logger.Info("Open file in explorer");
            var service = this.iocContainer.Resolve<IExternalProgramService>();
            service.OpenWinExplorerWithFileSelected(this.Notification.Folder + "\\" + this.Notification.File);
        }
    }
}
