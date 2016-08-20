using AutoMapper;
using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;
using Xunit;

namespace MvcTemplate.Tests.Unit.Data.Mapping
{
    public class ObjectMapperTests
    {
        static ObjectMapperTests()
        {
            ObjectMapper.MapObjects();
        }

        #region MapRoles()

        [Fact]
        public void MapRoles_Role_RoleView()
        {
            RoleView actual = Mapper.Map<RoleView>(new Role());

            Assert.NotNull(actual.Permissions);
        }

        [Fact]
        public void MapRoles_RoleView_Role()
        {
            Role actual = Mapper.Map<Role>(new RoleView());

            Assert.Empty(actual.Permissions);
        }

        #endregion
    }
}
