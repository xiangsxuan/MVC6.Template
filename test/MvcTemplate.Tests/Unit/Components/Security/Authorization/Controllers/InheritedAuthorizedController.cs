using Microsoft.AspNet.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public abstract class InheritedAuthorizedController : AuthorizedController
    {
        [HttpGet]
        public abstract ViewResult InheritanceAction();
    }
}
