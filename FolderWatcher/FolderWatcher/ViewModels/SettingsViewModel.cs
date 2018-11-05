using System;
using System.ComponentModel;
using System.Windows.Input;
using FolderWatcher.Helpers;
using FolderWatcher.Models;
using FolderWatcher.Services;
using Unity;

namespace FolderWatcher.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly IUnityContainer iocContainer;

        private readonly ISettingsService settingsService;

        private readonly Action<bool> settingsChangedCallback;

        private FolderWatcherSettings settings;

        private FolderDetails selectedFolder;

        private int meineSuperÄnderung; // neu

        public FolderWatcherSettings Settings
        {
            get
            {
                return this.settings;
            }

            set
            {
                if(this.settings != value)
                {
                    this.settings = value;
                    this.NotifyOfPropertyChanges("Settings");
                }
            }
        }

        public FolderDetails SelectedFolder
        {
            get
            {
                return this.selectedFolder;
            }

            set
            {
                if (this.selectedFolder != value)
                {
                    this.selectedFolder = value;
                    this.NotifyOfPropertyChanges("SelectedFolder");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SaveSettings { get; set; }

        public ICommand Cancel { get; set; }

        public ICommand AddFolder { get; set; }

        public ICommand RemoveFolder { get; set; }

        public SettingsViewModel(IUnityContainer container, Action<bool> settingsChangedCallback = null)
        {
            this.iocContainer = container;
            this.settingsService = container.Resolve<ISettingsService>();
            this.settingsChangedCallback = settingsChangedCallback;

            this.SaveSettings = new SimpleCommand(() => true, this.SaveChangedSettings);
            this.Cancel = new SimpleCommand(() => true, this.CloseWindow);
            this.AddFolder = new SimpleCommand(() => true, this.AddFolderEntry);
            this.RemoveFolder = new SimpleCommand(() => true, this.RemoveFolderEntry);

            this.Settings = this.settingsService.GetSettings();
        }

        private void SaveChangedSettings()
        {
            this.settingsService.SaveSettings(this.Settings);
            this.settingsChangedCallback?.Invoke(true);
            this.iocContainer.Resolve<IWindowManager>().CloseWindow(this.GetType());
        }

        private void CloseWindow()
        {
            this.settingsChangedCallback?.Invoke(false);
            this.iocContainer.Resolve<IWindowManager>().CloseWindow(this.GetType());
        }

        private void AddFolderEntry()
        {
            var newFolderEntry = new FolderDetails
            {
                FolderId = Guid.NewGuid(),
                FolderName = string.Empty
            };

            this.Settings.Folders.Add(newFolderEntry);

            this.SelectedFolder = newFolderEntry;
        }

        private void RemoveFolderEntry()
        {
            if (this.SelectedFolder != null)
            {
                this.Settings.Folders.Remove(this.SelectedFolder);
            }
        }

        private void NotifyOfPropertyChanges(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
