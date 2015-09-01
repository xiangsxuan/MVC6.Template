using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using MvcTemplate.Components.Security;
using System;

namespace MvcTemplate.Tests.Unit.Components.Security.Area
{
    [Authorize]
    [Area("Area")]
    public abstract class AuthorizedController : Controller
    {
        [HttpGet]
        public abstract ViewResult Action();

        [HttpPost]
        public abstract ViewResult Action(Object obj);

        [HttpGet]
        public abstract ViewResult AuthorizedGetAction();

        [HttpPost]
        [AllowAnonymous]
        public abstract ViewResult AuthorizedGetAction(Object obj);

        [HttpPost]
        public abstract ViewResult AuthorizedPostAction();

        [HttpGet]
        [ActionName("AuthorizedNamedGetAction")]
        public abstract ViewResult GetActionWithName();

        [HttpPost]
        [AllowAnonymous]
        [ActionName("AuthorizedNamedGetAction")]
        public abstract ViewResult GetActionWithName(Object obj);

        [HttpPost]
        [ActionName("AuthorizedNamedPostAction")]
        public abstract ViewResult PostActionWithName();

        [HttpGet]
        [AuthorizeAs("Action")]
        public abstract ViewResult AuthorizedAsAction();

        [HttpGet]
        [AuthorizeAs("InheritanceAction", Controller = "InheritedAuthorized", Area = "")]
        public abstract ViewResult AuthorizedAsOtherAction();
    }
}
