using System.Windows;
using FolderWatcher.Services;
using FolderWatcher.ViewModels;
using FolderWatcher.Views;
using Unity;

namespace FolderWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var container = new UnityContainer();
            container.RegisterType<ISettingsService, JsonSettingsService>();            

            var view = new MainView();
            view.DataContext = new MainViewModel(container);

            this.ContentView.Content = view;
        }        
    }
}
