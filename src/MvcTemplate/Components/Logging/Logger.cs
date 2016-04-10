using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace MvcTemplate.Components.Logging
{
    public class Logger : ILogger
    {
        private static Object LogWriting = new Object();
        private IConfiguration Config { get; }

        public Logger(IConfiguration config)
        {
            Config = config;
        }

        public void Log(String message)
        {
            Log(null, message);
        }
        public void Log(Int32? accountId, String message)
        {
            String logDirectory = Path.Combine(Config["Application:Path"], Config["Logger:Path"]);
            Int64 backupSize = Int64.Parse(Config["Logger:BackupSize"]);
            String logPath = Path.Combine(logDirectory, "Log.txt");

            StringBuilder log = new StringBuilder();
            log.AppendLine("Time   : " + DateTime.Now);
            log.AppendLine("Account: " + accountId);
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
    }
}
