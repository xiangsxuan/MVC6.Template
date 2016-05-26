using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [ExcludeFromCodeCoverage]
    public class InheritedAuthorizedController : AuthorizedController
    {
        [HttpGet]
        public ViewResult InheritanceAction()
        {
            return null;
        }
    }
}
