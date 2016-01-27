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
        private String AccountId { get; }
        private DbContext Context { get; }
        private Boolean Disposed { get; set; }

        public AuditLogger(DbContext context, String accountId)
        {
            Context = context;
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
            AccountId = !String.IsNullOrEmpty(accountId) ? accountId : null;
        }

        public void Log(IEnumerable<EntityEntry<BaseModel>> entries)
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
        public void Log(LoggableEntity entity)
        {
            AuditLog log = new AuditLog();
            log.Changes = entity.ToString();
            log.EntityName = entity.Name;
            log.Action = entity.Action;
            log.AccountId = AccountId;
            log.EntityId = entity.Id;

            Context.Add(log);
        }
        public void Save()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            if (Disposed) return;

            Context.Dispose();

            Disposed = true;
        }
    }
}
