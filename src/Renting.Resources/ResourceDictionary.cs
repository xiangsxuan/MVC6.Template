using System;
using System.Collections.Concurrent;

namespace Renting.Resources
{
    internal class ResourceDictionary : ConcurrentDictionary<String, String>
    {
        public ResourceDictionary()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
