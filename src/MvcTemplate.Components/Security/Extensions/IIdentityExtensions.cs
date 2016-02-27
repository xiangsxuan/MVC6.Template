using System;
using System.Security.Principal;

namespace MvcTemplate.Components.Security
{
    public static class IIdentityExtensions
    {
        public static Int32? Id(this IIdentity identity)
        {
            if (String.IsNullOrEmpty(identity.Name))
                return null;

            return Int32.Parse(identity.Name);
        }
    }
}
