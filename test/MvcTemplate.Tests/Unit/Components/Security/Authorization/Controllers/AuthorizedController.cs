using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using MvcTemplate.Components.Security;
using System;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [Authorize]
    public abstract class AuthorizedController : Controller
    {
        [HttpGet]
        public abstract ViewResult Action();

        [HttpPost]
        public abstract ViewResult Action(Object obj);

        [HttpGet]
        [AllowAnonymous]
        public abstract ViewResult AllowAnonymousAction();

        [HttpGet]
        [AllowUnauthorized]
        public abstract ViewResult AllowUnauthorizedAction();
    }
}
