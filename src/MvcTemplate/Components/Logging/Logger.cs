using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace MvcTemplate.Components.Logging
{
    public class Logger : ILogger
    {
        private Int32? AccountId { get; }
        private IConfiguration Config { get; }
        private static Object LogWriting = new Object();

        public Logger(IConfiguration config)
        {
            Config = config;
        }
        public Logger(IConfiguration config, Int32? accountId) : this(config)
        {
            AccountId = accountId;
        }

        public void Log(String message)
        {
            String logDirectory = Path.Combine(Config["Application:Path"], Config["Logger:Path"]);
            Int64 backupSize = Int64.Parse(Config["Logger:BackupSize"]);
            String logPath = Path.Combine(logDirectory, "Log.txt");

            StringBuilder log = new StringBuilder();
            log.AppendLine("Time   : " + DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss"));
            log.AppendLine("Account: " + AccountId);
            log.AppendLine("Message: " + message);
            log.AppendLine();

            lock (LogWriting)
            {
                Directory.CreateDirectory(logDirectory);
                File.AppendAllText(logPath, log.ToString());

                if (new FileInfo(logPath).Length >= backupSize)
                {
                    String logBackupFile = String.Format("Log {0}.txt", DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                    String backupPath = Path.Combine(logDirectory, logBackupFile);
                    File.Move(logPath, backupPath);
                }
            }
        }
        public void Log(Exception exception)
        {
            while (exception.InnerException != null)
                exception = exception.InnerException;

            Log($"{exception.GetType()}: {exception.Message}{Environment.NewLine}{exception.StackTrace}");
        }
    }
}
