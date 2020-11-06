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
            var file = GetFile();
            RefreshTheme(file);
        }

        public void DefaultTheme()
        {
            ThemeManager.ChangeAppStyle(_windowService.Window,
                ThemeManager.GetAccent("Blue"),
                ThemeManager.GetAppTheme("BaseDark"));
        }

        public void UpdateProperty(string propertyName, object value)
        {
            var file = GetFile();
            var config = file.ReadJson<ConfigApp>();
            config.GetType().GetProperty(propertyName)?.SetValue(config, value);
            config.WriteJson(file);
        }

        public ConfigApp GetConfiguration()
        {
            return GetFile().ReadJson<ConfigApp>();
        }

        private static void CreateFile(string path)
        {
            new ConfigApp
            {
                Theme = ConfigHelper.Theme,
                Printer = ConfigHelper.Printer,
                AccentColor = ConfigHelper.AccentColor,
                IsPrinterActivated = ConfigHelper.IsPrinterActivated,
            }.WriteJson(path);
        }

        private void RefreshTheme(string file)
        {
            var config = file.ReadJson<ConfigApp>();
            ThemeManager.ChangeAppStyle(_windowService.Window,
                ThemeManager.GetAccent(config.AccentColor),
                ThemeManager.GetAppTheme(config.Theme));
        }


        private static string GetFile()
        {
            var directory = ConfigHelper.Directory;
            directory = Path.Combine(directory, SystemDirectory.Configuration);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            var file = Path.Combine(directory, $"{nameof(ConfigApp)}.json");
            if (!File.Exists(file)) CreateFile(file);
            return file;
        }

    }
}
