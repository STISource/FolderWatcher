using System;
using System.Collections.Generic;
using System.Windows;
using FolderWatcher.ViewModels;
using FolderWatcher.Views;
using Hardcodet.Wpf.TaskbarNotification;


namespace FolderWatcher.Services
{
    public class WindowManager : IWindowManager
    {
        private readonly MainWindow shell;

        private readonly Dictionary<Type, Window> openWindows;

        private TaskbarIcon watcherTaskbarIcon;

        public WindowManager(MainWindow shell)
        {
            this.shell = shell;
            this.openWindows = new Dictionary<Type, Window>();
        }

        public void CloseWindow(Type viewModelType)
        {
            var window = this.openWindows[viewModelType];
            this.openWindows.Remove(viewModelType);
            window.Close();
        }

        public void NavigateToNewWindow(object viewModel)
        {
            var viewModelType = viewModel.GetType();
            var viewObjectHandle = Activator.CreateInstance(null, this.GetViewFullName(viewModelType));
            var view = (Window)viewObjectHandle.Unwrap();

            view.DataContext = viewModel;
            view.Show();

            if(this.openWindows.ContainsKey(viewModelType))
            {
                var window = this.openWindows[viewModelType];
                this.openWindows.Remove(viewModelType);
                window.Close();
            }
            this.openWindows.Add(viewModelType, view);
        }

        public void LoadMainView(MainViewModel viewModel)
        {   
            var view = new MainView();

            view.DataContext = viewModel;
            this.shell.ContentView = view;

            this.watcherTaskbarIcon = view.WatcherTaskbarIcon;
        }

        private string GetViewFullName(Type viewModelType)
        {
            var viewClassName = viewModelType.Name.Replace("ViewModel", "View");
            var viewNameSpace = viewModelType.Namespace.Replace("ViewModels", "Views");

            return string.Format("{0}.{1}", viewNameSpace, viewClassName);
        }

        public void ShowBalloon(string title, string message)
        {
            this.watcherTaskbarIcon.ShowBalloonTip(title, message, BalloonIcon.Info);
        }
    }
}
