using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MvcTemplate.Components.Extensions;
using System;
using System.IO;
using System.Text;

namespace MvcTemplate.Components.Logging
{
    public class Logger : ILogger
    {
        private IConfiguration Config { get; }
        private Func<Int32?> AccountId { get; }
        private static Object LogWriting { get; } = new Object();

        public Logger(IConfiguration config)
        {
            IHttpContextAccessor accessor = new HttpContextAccessor();
            AccountId = () => accessor.HttpContext?.User.Id();
            Config = config;
        }
        public Logger(IConfiguration config, Int32? accountId)
        {
            AccountId = () => accountId;
            Config = config;
        }

        public void Log(String message)
        {
            String logDirectory = Path.Combine(Config["Application:Path"], Config["Logger:Directory"]);
            Int64 backupSize = Int64.Parse(Config["Logger:BackupSize"]);
            String path = Path.Combine(logDirectory, "Log.txt");

            StringBuilder log = new StringBuilder();
            log.AppendLine("Time   : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            log.AppendLine("Account: " + AccountId());
            log.AppendLine("Message: " + message);
            log.AppendLine();

            lock (LogWriting)
            {
                Directory.CreateDirectory(logDirectory);
                File.AppendAllText(path, log.ToString());

                if (new FileInfo(path).Length >= backupSize)
                    File.Move(path, Path.Combine(logDirectory, $"Log {DateTime.Now:yyyy-MM-dd HHmmss}.txt"));
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
