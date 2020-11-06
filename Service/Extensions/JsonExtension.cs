using Newtonsoft.Json;
using System.IO;

namespace Service.Extensions
{
    internal static class JsonExtension
    {

        public static void WriteJson<T>(this T model, string path)
        {
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static T ReadJson<T>(this string path)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
    }
}
