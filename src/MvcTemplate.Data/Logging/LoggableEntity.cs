using Microsoft.Data.Entity.ChangeTracking;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MvcTemplate.Data.Logging
{
    public class LoggableEntity
    {
        public String Id { get; }
        public String Name { get; }
        public String Action { get; }

        public IEnumerable<LoggableProperty> Properties { get; }

        public LoggableEntity(EntityEntry<BaseModel> entry)
        {
            Type entityType = entry.Entity.GetType();
            Properties = new LoggableProperty[0];
            Action = entry.State.ToString();
            Name = entityType.Name;
            Id = entry.Entity.Id;
        }

        public override String ToString()
        {
            StringBuilder changes = new StringBuilder();
            foreach (LoggableProperty property in Properties)
                changes.Append(property + Environment.NewLine);

            return changes.ToString();
        }
    }
}
