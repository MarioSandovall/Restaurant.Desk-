using System.Configuration;

namespace Service.Utils
{
    internal static class ConfigHelper
    {
        public static string Server => string.CompareOrdinal(ConfigurationManager.AppSettings["Environment"], "Production") == 0
               ? ConfigurationManager.AppSettings["ProductionServer"]
               : ConfigurationManager.AppSettings["TestServer"];
    }
}
