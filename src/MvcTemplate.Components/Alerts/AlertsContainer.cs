using System;
using System.Collections.Generic;

namespace MvcTemplate.Components.Alerts
{
    public class AlertsContainer : List<Alert>
    {
        public void Add(AlertType type, String message)
        {
            Add(type, message, 4000);
        }
        public void Add(AlertType type, String message, Int32 timeout)
        {
            Add(new Alert
            {
                Type = type,
                Message = message,
                Timeout = timeout
            });
        }

        public void AddError(String message)
        {
            AddError(message, 0);
        }
        public void AddError(String message, Int32 timeout)
        {
            Add(AlertType.Danger, message, timeout);
        }

        public void Merge(AlertsContainer alerts)
        {
            if (alerts == this) return;

            AddRange(alerts);
        }
    }
}
