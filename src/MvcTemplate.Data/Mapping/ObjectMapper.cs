using AutoMapper;
using MvcTemplate.Objects;

namespace MvcTemplate.Data.Mapping
{
    public static class ObjectMapper
    {
        public static void MapObjects()
        {
            MapRoles();
            MapAccounts();

            Mapper.Configuration.Seal();
        }

        #region Administration

        private static void MapRoles()
        {
            Mapper.CreateMap<Role, RoleView>()
                .ForMember(role => role.Permissions, member => member.Ignore());
            Mapper.CreateMap<RoleView, Role>()
                .ForMember(role => role.Permissions, member => member.Ignore());
        }
        private static void MapAccounts()
        {
            Mapper.CreateMap<Account, AccountView>();
            Mapper.CreateMap<Account, AccountEditView>();
            Mapper.CreateMap<Account, ProfileEditView>();

            Mapper.CreateMap<AccountCreateView, Account>();
            Mapper.CreateMap<AccountRegisterView, Account>();
        }

        #endregion
    }
}
