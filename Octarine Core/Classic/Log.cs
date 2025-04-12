using System;
using System.IO;

namespace Octarine_Core.Classic
{
    public class Log
    {
        private readonly string _logFilePath = "client_logs.txt"; // Файл для логов звонкоа
        private readonly string _logFilePathport = "ports_logs.txt"; // Файл для логов с портами
        private readonly string _logFilePathEx = "exeprion_logs.txt"; // Файл для логов с портами

        public void log(string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            try
            {
                File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка записи в лог-файл: {ex.Message}");
            }
        }
        public void log1(string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            try
            {
                File.AppendAllText(_logFilePathport, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка записи в лог-файл: {ex.Message}");
            }

        }
        public void Ex(string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            try
            {
                File.AppendAllText(_logFilePathEx, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка записи в лог-файл: {ex.Message}");
            }
        }

    }
}
