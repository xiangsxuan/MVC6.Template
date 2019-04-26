using Microsoft.EntityFrameworkCore.ChangeTracking;
using Renting.Objects;
using System;
using System.Collections.Generic;

namespace Renting.Data.Logging
{
    public interface IAuditLogger : IDisposable
    {
        void Log(IEnumerable<EntityEntry<BaseModel>> entries);
        void Save();
    }
}
