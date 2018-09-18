using FolderWatcher.ViewModels;
using System;

namespace FolderWatcher.Services
{
    public interface IWindowManager
    {        
        void LoadMainView(MainViewModel viewModel);

        void NavigateToNewWindow(object viewModel);

        void CloseWindow(Type viewModelType);

        void ShowBalloon(string title, string message);
    }
}
