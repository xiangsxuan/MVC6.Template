﻿using System;
using System.Security.Claims;

namespace Renting.Components.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Int32? Id(this ClaimsPrincipal principal)
        {
            String id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (String.IsNullOrEmpty(id))
                return null;

            return Int32.Parse(id);
        }
        public static String Email(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.Email)?.Value;
        }
        public static String Username(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static void UpdateClaim(this ClaimsPrincipal principal, String type, String value)
        {
            if (principal.Identity is ClaimsIdentity identity)
            {
                identity.TryRemoveClaim(identity.FindFirst(type));
                identity.AddClaim(new Claim(type, value));
            }
        }
    }
}
