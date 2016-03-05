using MvcTemplate.Components.Security;
using NSubstitute;
using System;
using System.Security.Principal;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public class IIdentityExtensionsTests
    {
        #region Extension method: Id(this IIdentity identity)

        [Theory]
        [InlineData("1", 1)]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void Id_ReturnsEntityNameAsInteger(String identityName, Int32? id)
        {
            IIdentity identity = Substitute.For<IIdentity>();
            identity.Name.Returns(identityName);

            Int32? actual = identity.Id();
            Int32? expected = id;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
