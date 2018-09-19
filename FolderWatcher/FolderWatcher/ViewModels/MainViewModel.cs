using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using FolderWatcher.Helpers;
using FolderWatcher.Models;
using FolderWatcher.Properties;
using FolderWatcher.Services;
using FolderWatcher.Worker;
using NLog;
using Unity;
using Unity.Interception.Utilities;

namespace FolderWatcher.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IUnityContainer iocContainer;

        private readonly INotificationHistoryService notificationHistoryService;

        private FolderWatcherSettings settings;

        private IWindowManager windowManager;

        private IEnumerable<IFolderWatcher> watchers;        

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<NotificationViewModel> Notifications { get; set; }

        public ICommand DismissAll { get; set; }

        public ICommand OpenSettings { get; set; }

        public ICommand ExitApplication { get; set; }

        public string ToolTip
        {
            get
            {
                return this.Notifications.Any()
                    ? string.Format(Resources.NewFilesNotification, this.Notifications.Count())
                    : Resources.NoNewFilesNotification;
            }
        }

        public MainViewModel(IUnityContainer iocContainer)
        {
            this.iocContainer = iocContainer;
            this.Notifications = new ObservableCollection<NotificationViewModel>();
            this.notificationHistoryService = iocContainer.Resolve<INotificationHistoryService>();
            this.DismissAll = new SimpleCommand(() => true, this.DismissAllNotifications);
            this.OpenSettings = new SimpleCommand(() => true, this.DisplaySettings);
            this.ExitApplication = new SimpleCommand(() => true, this.ExitRunningApplication);
            this.windowManager = iocContainer.Resolve<IWindowManager>();
            
            this.ReapplySettings();            
        }        

        private void ReapplySettings()
        {
            logger.Info("Reapplying settings ...");
            // clear everything that has been wired up so far
            this.StopWatching();

            this.settings = this.iocContainer.Resolve<ISettingsService>().GetSettings();

            // add watchers
            var watchers = new List<IFolderWatcher>();

            foreach (var folder in this.settings.Folders)
            {
                var watcher = this.iocContainer.Resolve<IFolderWatcher>();
                watcher.FolderContentChanged += FolderContentChanged;
                watcher.Watch(folder);

                watchers.Add(watcher);
            }

            this.watchers = watchers;

            // load unread notifications            
            var notifications = this.notificationHistoryService.ReadAll();
            foreach(var notification in notifications)
            {
                var vm = new NotificationViewModel(notification, this.iocContainer);
                vm.OnDismiss += this.NotificationDismissed;                
                this.Notifications.Add(vm);
            }
        }

        private void FolderContentChanged(object sender, FolderContentChangedEventArgs args)
        {
            logger.Info("Folder content changed. Files have been added: {0}", string.Join(", ", args.CreatedFiles.Select(x => x.FileName)));
            foreach(var file in args.CreatedFiles)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    var notification = new NewFileNotification
                                            {
                                                ChangeDate = file.ChangeDate,
                                                Folder = settings.Folders.Single(x => x.FolderId == file.FolderId).FolderName,
                                                File = file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1)
                                            };

                    var vm = new NotificationViewModel(notification, this.iocContainer);
                    vm.OnDismiss += this.NotificationDismissed;
                    this.notificationHistoryService.Create(notification);
                    this.Notifications.Add(vm);                    
                });
            }

            // remove notifications for files already removed 
            var notificationsToRemove = this.Notifications.Where(x => !System.IO.File.Exists(x.Notification.Folder + "\\" + x.Notification.File)).ToList();
            foreach(var vm in notificationsToRemove)
            {
                logger.Info("Removing notification for deleted file: {0}", vm.Notification.File);
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    vm.DismissNotification();
                });                
            }

            this.windowManager.ShowBalloon(Resources.NewFilesDetected_Title, string.Join(Environment.NewLine, args.CreatedFiles.Select(x => x.FileName)));

            this.NotifyOfPropertyChanges("ToolTip");
        }

        private void DismissAllNotifications()
        {
            logger.Info("Dismiss all notifications");
            foreach(var vm in this.Notifications)
            {
                vm.OnDismiss -= this.NotificationDismissed;
                this.notificationHistoryService.Delete(vm.Notification);
            }

            this.Notifications.Clear();

            this.NotifyOfPropertyChanges("ToolTip");
        }

        private void DisplaySettings()
        {            
            this.windowManager.NavigateToNewWindow(new SettingsViewModel(this.iocContainer, SettingsChanged));
        }

        private void SettingsChanged(bool settingsHaveBeenSaved)
        {
            if (settingsHaveBeenSaved)
            {
                this.ReapplySettings();
            }
        }

        private void StopWatching()
        {
            this.Notifications.ForEach(x => x.OnDismiss -= this.NotificationDismissed);
            this.Notifications.Clear();

            if (this.watchers != null && this.watchers.Any())
            {
                foreach (var watcher in this.watchers)
                {
                    watcher.StopWatching();
                    watcher.FolderContentChanged -= FolderContentChanged;
                }
            }
        }

        private void NotificationDismissed(object sender, EventArgs e)
        {
            var vm = (NotificationViewModel)sender;
            vm.OnDismiss -= this.NotificationDismissed;
            this.notificationHistoryService.Delete(vm.Notification);
            this.Notifications.Remove(vm);

            this.NotifyOfPropertyChanges("ToolTip");
        }

        private void NotifyOfPropertyChanges(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExitRunningApplication()
        {            
            App.Current.Shutdown();            
        }
    }
}
