using System;

namespace MvcTemplate.Components.Logging
{
    public interface ILogger
    {
        void Log(String message);
        void Log(Int32? accountId, String message);
    }
}
