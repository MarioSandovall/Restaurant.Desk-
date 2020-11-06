using System;
using System.Configuration;

namespace Service.Utils
{
    internal static class ConfigHelper
    {
        public static string Theme => ConfigurationManager.AppSettings["Theme"];

        public static string Printer => ConfigurationManager.AppSettings["Printer"];

        public static string AccentColor => ConfigurationManager.AppSettings["AccentColor"];

        public static bool IsPrinterActivated => Convert.ToBoolean(ConfigurationManager.AppSettings["IsPrinterActivated"]);

        public static string Directory => ConfigurationManager.AppSettings["Directory"];

        public static string Server => string.CompareOrdinal(ConfigurationManager.AppSettings["Environment"], "Production") == 0
               ? ConfigurationManager.AppSettings["ProductionServer"]
               : ConfigurationManager.AppSettings["TestServer"];
    }
}
