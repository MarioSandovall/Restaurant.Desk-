using Autofac;
using Business.Startup;
using Business.ViewModels.Main;
using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Wpf.Views;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel _mainViewModel;
        private static readonly Mutex Mutex = new Mutex(true, Assembly.GetExecutingAssembly().GetName().CodeBase);

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!Mutex.WaitOne(TimeSpan.Zero, true))
            {
                MessageBox.Show("The application is already running");
                Current.Shutdown();
            }

            var container = Bootstrapper.Instance.BootStrap();
            var mainWindow = new MainWindow();
            _mainViewModel = container.Resolve<MainViewModel>();

            mainWindow.DataContext = _mainViewModel;
            mainWindow.Loaded += (sender, args) => _mainViewModel.Load();
            mainWindow.Closing += (s, a) =>
            {
                a.Cancel = true;
                _mainViewModel.CloseApplication();
            };

            _mainViewModel.Theme();
            mainWindow.Show();

            base.OnStartup(e);
        }


        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            MessageBox.Show("Error: Please contact your system administrator"
                            + Environment.NewLine + Environment.NewLine +
                            "Error Message: " + e.Exception.Message);
        }
    }
}
