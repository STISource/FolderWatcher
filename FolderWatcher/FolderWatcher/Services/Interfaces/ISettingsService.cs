using FolderWatcher.Models;

namespace FolderWatcher.Services
{
    public interface ISettingsService
    {
        FolderWatcherSettings GetSettings();

        void SaveSettings(FolderWatcherSettings settings);
    }
}
