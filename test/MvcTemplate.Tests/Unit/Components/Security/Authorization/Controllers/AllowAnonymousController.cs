using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using MvcTemplate.Components.Security;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [AllowAnonymous]
    public abstract class AllowAnonymousController : AuthorizedController
    {
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        [AllowUnauthorized]
        public abstract ViewResult AuthorizedAction();
    }
}
