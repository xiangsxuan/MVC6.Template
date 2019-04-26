using System.Diagnostics.CodeAnalysis;

namespace Renting.Components.Security.Tests
{
    [AllowUnauthorized]
    [ExcludeFromCodeCoverage]
    public class AllowUnauthorizedController : AuthorizedController
    {
    }
}
