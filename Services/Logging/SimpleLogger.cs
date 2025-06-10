using System;
using System.IO;

namespace MalshinonApp.Services.Logging
{
    public static class SimpleLogger
    {
        private static readonly string LogFilePath = "Logs/log.txt";

        static SimpleLogger()
        {
            // Ensure the Logs folder exists
            Directory.CreateDirectory("Logs");
        }

        public static void LogInfo(string message)
        {
            Log("INFO", message);
        }

        public static void LogError(string message)
        {
            Log("ERROR", message);
        }

        public static void Log(string level, string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
        }
    }
}