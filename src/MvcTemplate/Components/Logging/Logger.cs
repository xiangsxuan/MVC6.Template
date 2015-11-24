using Microsoft.Data.Entity;
using MvcTemplate.Objects;
using System;

namespace MvcTemplate.Components.Logging
{
    public class Logger : ILogger
    {
        private DbContext Context { get; }
        private Boolean Disposed { get; set; }

        public Logger(DbContext context)
        {
            Context = context;
        }

        public virtual void Log(String message)
        {
            Log(null, message);
        }
        public virtual void Log(String accountId, String message)
        {
            Log log = new Log();
            log.Message = message;
            log.AccountId = !String.IsNullOrEmpty(accountId) ? accountId : null;

            Context.Add(log);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            Context.Dispose();

            Disposed = true;
        }
    }
}
