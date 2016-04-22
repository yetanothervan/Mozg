using System.Windows;
using WinApp.Unity;

namespace WinApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var btsr = new MyBootstrapper();
            btsr.Run();
        }
    }
}
