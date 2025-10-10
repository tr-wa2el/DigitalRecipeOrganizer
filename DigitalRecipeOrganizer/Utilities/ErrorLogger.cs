using System.Text;

namespace DigitalRecipeOrganizer.Utilities
{
    public static class ErrorLogger
    {
        private static readonly string LogFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DigitalRecipeOrganizer",
            "error.log"
        );

        static ErrorLogger()
        {
            // Ensure log directory exists
            string? logDirectory = Path.GetDirectoryName(LogFilePath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public static void LogError(Exception ex, string context = "")
        {
            try
            {
                StringBuilder logEntry = new StringBuilder();
                logEntry.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR");
                
                if (!string.IsNullOrEmpty(context))
                {
                    logEntry.AppendLine($"Context: {context}");
                }
                
                logEntry.AppendLine($"Message: {ex.Message}");
                logEntry.AppendLine($"Type: {ex.GetType().Name}");
                
                if (ex.InnerException != null)
                {
                    logEntry.AppendLine($"Inner Exception: {ex.InnerException.Message}");
                }
                
                logEntry.AppendLine($"Stack Trace: {ex.StackTrace}");
                logEntry.AppendLine(new string('-', 80));
                
                File.AppendAllText(LogFilePath, logEntry.ToString());
            }
            catch
            {
                // Fail silently - don't let logging errors crash the app
            }
        }

        public static void LogInfo(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO: {message}\n";
                File.AppendAllText(LogFilePath, logEntry);
            }
            catch
            {
                // Fail silently
            }
        }

        public static string GetLogFilePath()
        {
            return LogFilePath;
        }

        public static void ClearLog()
        {
            try
            {
                if (File.Exists(LogFilePath))
                {
                    File.Delete(LogFilePath);
                }
            }
            catch
            {
                // Fail silently
            }
        }
    }
}
