using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcTemplate.Data.Logging
{
    public class AuditLogger : IAuditLogger
    {
        private DbContext Context { get; set; }
        private String AccountId { get; set; }
        private Boolean Disposed { get; set; }

        public AuditLogger(DbContext context, String accountId = null)
        {
            Context = context;
            AccountId = accountId;
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public virtual void Log(IEnumerable<EntityEntry<BaseModel>> entries)
        {
            foreach (EntityEntry<BaseModel> entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                    case EntityState.Modified:
                        LoggableEntity entity = new LoggableEntity(entry);
                        if (entity.Properties.Any())
                            Log(entity);
                        break;
                }
            }
        }
        public virtual void Log(LoggableEntity entity)
        {
            AuditLog log = new AuditLog();
            log.AccountId = !String.IsNullOrEmpty(log.AccountId) ? log.AccountId : null;
            log.Changes = entity.ToString();
            log.EntityName = entity.Name;
            log.Action = entity.Action;
            log.EntityId = entity.Id;

            Context.Set<AuditLog>().Add(log);
        }
        public virtual void Save()
        {
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
