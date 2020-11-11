using MahApps.Metro;
using Model.Models;
using Model.Utils;
using Service.Extensions;
using Service.Interfaces;
using Service.Utils;
using System.IO;

namespace Service.Services
{
    public class ConfigService : IConfigService
    {
        private readonly IWindowService _windowService;
        public ConfigService(
            IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void LoadTheme()
        {
            var filePath = GetConfigFilePath();

            RefreshTheme(filePath);
        }

        public void DefaultTheme()
        {
            ThemeManager.ChangeAppStyle(_windowService.Window,
                ThemeManager.GetAccent("Blue"),
                ThemeManager.GetAppTheme("BaseDark"));
        }

        public void UpdateProperty(string propertyName, object value)
        {
            var filePath = GetConfigFilePath();

            var configApp = filePath.ReadJson<ConfigApp>();

            configApp.GetType().GetProperty(propertyName)?.SetValue(configApp, value);

            configApp.WriteJson(filePath);
        }

        public ConfigApp GetConfiguration()
        {
            return GetConfigFilePath().ReadJson<ConfigApp>();
        }

        private static void CreateFile(string path)
        {
            var configApp = new ConfigApp
            {
                Theme = ConfigHelper.Theme,
                Printer = ConfigHelper.Printer,
                AccentColor = ConfigHelper.AccentColor,
                IsPrinterActivated = ConfigHelper.IsPrinterActivated
            };

            configApp.WriteJson(path);
        }

        private void RefreshTheme(string filePath)
        {
            var configApp = filePath.ReadJson<ConfigApp>();

            var newTheme = ThemeManager.GetAppTheme(configApp.Theme);
            var newAccent = ThemeManager.GetAccent(configApp.AccentColor);

            ThemeManager.ChangeAppStyle(_windowService.Window, newAccent, newTheme);
        }


        private static string GetConfigFilePath()
        {
            var directoryPath = ConfigHelper.Directory;

            directoryPath = Path.Combine(directoryPath, SystemDirectory.Configuration);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            var filePath = Path.Combine(directoryPath, $"{nameof(ConfigApp)}.json");
            if (!File.Exists(filePath)) CreateFile(filePath);

            return filePath;
        }

    }
}
