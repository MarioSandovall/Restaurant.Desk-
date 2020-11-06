using MahApps.Metro.Controls;
using Service.Interfaces;
using System.Windows;

namespace Service.Services
{
    public class WindowService : IWindowService
    {
        public MetroWindow Window => (MetroWindow)Application.Current.MainWindow;
    }
}
