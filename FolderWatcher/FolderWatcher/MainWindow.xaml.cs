using System.Windows;
using FolderWatcher.Services;
using FolderWatcher.ViewModels;
using FolderWatcher.Worker;
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
            container.RegisterInstance<ISettingsService>(new JsonSettingsService());
            container.RegisterInstance<IFolderContentSnapshotService>(new NoSqlSnapshotService());
            container.RegisterInstance<INotificationHistoryService>(new NoSqlNotificationHistoryService());
            container.RegisterInstance<IWindowManager>(new WindowManager(this));
            container.RegisterType<IFolderWatcher, ComparativeFolderWatcher>();

            container.Resolve<IWindowManager>().LoadMainView(new MainViewModel(container));

            this.Hide();
        }        
    }
}
