using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MvcTemplate.Resources
{
    internal class ResourceDictionary : ConcurrentDictionary<String, String>
    {
        public ResourceDictionary()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
