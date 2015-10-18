using Microsoft.Data.Entity.ChangeTracking;
using Newtonsoft.Json;
using System;

namespace MvcTemplate.Data.Logging
{
    public class LoggableProperty
    {
        public Boolean IsModified { get; }
        private String PropertyName { get; }
        private Object CurrentValue { get; }
        private Object OriginalValue { get; }

        public LoggableProperty(PropertyEntry entry, Object originalValue)
        {
            OriginalValue = originalValue;
            CurrentValue = entry.CurrentValue;
            PropertyName = entry.Metadata.Name;
            IsModified = entry.IsModified && !Equals(OriginalValue, CurrentValue);
        }

        public override String ToString()
        {
            if (IsModified)
                return PropertyName + ": " + Format(OriginalValue) + " => " + Format(CurrentValue);

            return PropertyName + ": " + Format(OriginalValue);
        }

        private String Format(Object value)
        {
            if (value == null)
                return "null";

            if (value is DateTime?)
                return "\"" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "\"";

            return JsonConvert.ToString(value);
        }
    }
}
