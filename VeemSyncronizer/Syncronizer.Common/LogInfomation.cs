using System;
using System.IO;

namespace Syncronizer.Common
{
    internal class LogInfomation
    {
        public enum LogLevel { Debug, Info, Warning, None }

        private readonly string logFilePath;
        private readonly string logFileName = "log.log";
        private readonly string logPathFailSafe = Directory.GetCurrentDirectory();

        public LogInfomation(string logFilePath = null)
        {
            logFilePath = logFilePath ?? Path.Combine(logPathFailSafe, logFileName);
            Log(message: "Log have been started!");
        }
        public void Log(string message, LogLevel logLevel = LogLevel.Info)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            WriteLog(message);
            Console.ResetColor();
        }

        private void WriteLog(string message)
        {            
            message += $"\t{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            // Write to file

            if (!File.Exists(logFilePath))
            {
                message += $"\nFile not exists, file will be created on desired path: {logFilePath}";

                using (var sw = File.CreateText(logFilePath))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (var sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(message);
                }
            }

            Console.WriteLine(message);
        }
    }
}
