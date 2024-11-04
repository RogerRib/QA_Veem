using System;
using System.IO;

namespace Syncronizer.Common
{
    public class LogInfomation
    {
        public enum LogLevel { Info, Warning, Error, None }

        private readonly string _logFilePath;
        private readonly string _logFileName = "log.log";
        private readonly string _logPathFailSafe = Directory.GetCurrentDirectory();

        public LogInfomation(string logFilePath = null)
        {
            _logFilePath = logFilePath?? Path.Combine(_logPathFailSafe, _logFileName);


            Log(message: "Log have been started!");
        }
        public void Log(string message, LogLevel logLevel = LogLevel.Info)
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            WriteLog(message, logLevel);
            Console.ResetColor();
        }

        private void WriteLog(string incomingMessage, LogLevel logLevel = LogLevel.Info)
        {
            //string message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t" +logLevel+ incomingMessage; 
            string message = string.Format("{0:HH:mm:ss:fff}, {1}, {2}", DateTime.Now, logLevel, incomingMessage);
            // Write to file

            if (!File.Exists(_logFilePath))
            {
                message += $"\nFile not exists, file will be created on desired path: {_logFilePath}";

                using (var sw = File.CreateText(_logFilePath))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (var sw = File.AppendText(_logFilePath))
                {
                    sw.WriteLine(message);
                }
            }

            Console.WriteLine(message);
        }
    }
}
