using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcTemplate.Components.Security;
using System.Diagnostics.CodeAnalysis;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [AllowAnonymous]
    [ExcludeFromCodeCoverage]
    public class AllowAnonymousController : AuthorizedController
    {
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        [AllowUnauthorized]
        public ViewResult AuthorizedAction()
        {
            return null;
        }
    }
}
