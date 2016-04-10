using Microsoft.Extensions.Configuration;
using MvcTemplate.Components.Logging;
using System;
using System.IO;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Logging
{
    public class LoggerTests
    {
        private static IConfiguration config;
        private String logDirectory;
        private Int32 backupSize;
        private String logPath;
        private Logger logger;

        static LoggerTests()
        {
            config = ConfigurationFactory.Create();
        }
        public LoggerTests()
        {
            logPath = Path.Combine(config["Application:Path"], config["Logger:Path"], "Log.txt");
            logDirectory = Path.Combine(config["Application:Path"], config["Logger:Path"]);
            backupSize = Int32.Parse(config["Logger:BackupSize"]);
            logger = new Logger(config);

            if (Directory.Exists(logDirectory))
                Directory.Delete(logDirectory, true);
        }

        #region Log(String message)

        [Fact]
        public void Log_LogsMessage()
        {
            logger.Log("Test");

            String expected = "Account: " + Environment.NewLine + "Message: Test" + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(logPath);

            Assert.True(actual.StartsWith("Time   :"));
            Assert.True(actual.EndsWith(expected));
        }

        [Fact]
        public void Log_OnExceededLogSizeCreatesABackupFile()
        {
            logger.Log(new String('T', backupSize));

            String expected = "Account: " + Environment.NewLine + "Message: " + new String('T', backupSize) + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(Path.Combine(logDirectory, String.Format("Log {0}.txt", DateTime.Now.ToString("yyyy-MM-dd HHmmss"))));

            Assert.True(actual.StartsWith("Time   :"));
            Assert.True(actual.EndsWith(expected));
        }

        #endregion

        #region Log(String accountId, String message)

        [Fact]
        public void Log_LogsAccountIdAndMessage()
        {
            logger.Log(1, "MessageTest");

            String expected = "Account: 1" + Environment.NewLine + "Message: MessageTest" + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(logPath);

            Assert.True(actual.StartsWith("Time   :"));
            Assert.True(actual.EndsWith(expected));
        }

        #endregion
    }
}
