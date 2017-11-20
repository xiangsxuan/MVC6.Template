using Microsoft.Extensions.Configuration;
using MvcTemplate.Components.Logging;
using NSubstitute;
using System;
using System.IO;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Logging
{
    public class LoggerTests
    {
        private IConfiguration config;
        private String logDirectory;
        private Int32 backupSize;
        private String log;

        public LoggerTests()
        {
            config = ConfigurationFactory.Create();
            backupSize = Int32.Parse(config["Logger:BackupSize"]);
            logDirectory = Path.Combine(config["Application:Path"], config["Logger:Directory"]);
            log = Path.Combine(config["Application:Path"], config["Logger:Directory"], "Log.txt");

            if (Directory.Exists(logDirectory))
                Directory.Delete(logDirectory, true);
        }

        #region Log(String message)

        [Fact]
        public void Log_LogsMessage()
        {
            Logger logger = new Logger(config);

            logger.Log("Test");

            String expected = "Account: " + Environment.NewLine + "Message: Test" + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(log);

            Assert.StartsWith("Time   :", actual);
            Assert.EndsWith(expected, actual);
        }

        [Fact]
        public void Log_CreatesBackupFile()
        {
            Logger logger = new Logger(config, 2);

            logger.Log(new String('T', backupSize));

            String expected = "Account: 2" + Environment.NewLine + "Message: " + new String('T', backupSize) + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(Path.Combine(logDirectory, $"Log {DateTime.Now:yyyy-MM-dd HHmmss}.txt"));

            Assert.StartsWith("Time   :", actual);
            Assert.EndsWith(expected, actual);
        }

        #endregion

        #region Log(Exception exception)

        [Fact]
        public void Log_InnerException()
        {
            Exception exception = new Exception("", Substitute.ForPartsOf<Exception>());
            exception.InnerException.StackTrace.Returns("StackTrace");
            exception.InnerException.Message.Returns("Message");
            Logger logger = new Logger(config, 2);

            logger.Log(exception);

            String actual = File.ReadAllText(log);
            String expected = String.Format("Account: 2{0}Message: {1}: {2}{0}{3}{0}{0}",
                 Environment.NewLine,
                 exception.InnerException.GetType(),
                 exception.InnerException.Message,
                 exception.InnerException.StackTrace);

            Assert.StartsWith("Time   :", actual);
            Assert.EndsWith(expected, actual);
        }

        #endregion
    }
}
