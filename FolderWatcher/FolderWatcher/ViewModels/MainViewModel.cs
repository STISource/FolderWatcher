using FolderWatcher.Models;
using FolderWatcher.Services;
using Unity;

namespace FolderWatcher.ViewModels
{
    public class MainViewModel
    {
        private readonly IUnityContainer iocContainer;
        private readonly FolderWatcherSettings settings;

        public MainViewModel(IUnityContainer iocContainer)
        {
            this.iocContainer = iocContainer;
            this.settings = this.iocContainer.Resolve<ISettingsService>().GetSettings();
        }
    }
}
