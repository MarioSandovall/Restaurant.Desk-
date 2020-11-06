using Model.Utils;
using Service.Interfaces;
using Service.Utils;
using System;
using System.IO;

namespace Service.Services
{
    public class LogService : ILogService
    {
        public void Write(Exception ex)
        {
            var directory = ConfigHelper.Directory;
            directory = Path.Combine(directory, SystemDirectory.Log);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            var filePath = Path.Combine(directory, $"{DateTime.Now:yyyMMdd}.log");
            if (!File.Exists(filePath)) File.Create(filePath).Dispose();

            using (var write = new StreamWriter(filePath, true))
            {
                write.WriteLine("Message: " + ex.Message + Environment.NewLine +
                            "StackTrace: " + ex.StackTrace + Environment.NewLine +
                            "Time: " + DateTime.Now.ToString("h:mm:ss tt zz"));
                write.Write(Environment.NewLine + "------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }
}

