using Mvvm.Core.Services;
using System.Windows;

namespace Lab16_Folders
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow()
            {
                DataContext = new MainViewModel(new RelayCommandFactory())
            };
            window.Show();
            base.OnStartup(e);
        }
    }
}