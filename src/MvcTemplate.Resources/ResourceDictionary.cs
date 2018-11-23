using System;
using System.Collections.Generic;

namespace MvcTemplate.Resources
{
    internal class ResourceDictionary : Dictionary<String, String>
    {
        public ResourceDictionary()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
