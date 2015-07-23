using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using MvcTemplate.Data.Core;
using System;

namespace MvcTemplate.Data.Migrations
{
    [ContextType(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        public override void BuildModel(ModelBuilder builder)
        {
            builder.Entity("MvcTemplate.Objects.Account", b =>
                {
                    b.Property<string>("Id");
                    
                    b.Property<DateTime>("CreationDate");
                    
                    b.Property<string>("Email");
                    
                    b.Property<bool>("IsLocked");
                    
                    b.Property<string>("Passhash");
                    
                    b.Property<string>("RecoveryToken");
                    
                    b.Property<DateTime?>("RecoveryTokenExpirationDate");
                    
                    b.Property<string>("RoleId");
                    
                    b.Property<string>("Username");
                    
                    b.Key("Id");
                });
            
            builder.Entity("MvcTemplate.Objects.AuditLog", b =>
                {
                    b.Property<string>("Id");
                    
                    b.Property<string>("AccountId");
                    
                    b.Property<string>("Action");
                    
                    b.Property<string>("Changes");
                    
                    b.Property<DateTime>("CreationDate");
                    
                    b.Property<string>("EntityId");
                    
                    b.Property<string>("EntityName");
                    
                    b.Key("Id");
                });
            
            builder.Entity("MvcTemplate.Objects.Log", b =>
                {
                    b.Property<string>("Id");
                    
                    b.Property<string>("AccountId");
                    
                    b.Property<DateTime>("CreationDate");
                    
                    b.Property<string>("Message");
                    
                    b.Key("Id");
                });
            
            builder.Entity("MvcTemplate.Objects.Privilege", b =>
                {
                    b.Property<string>("Id");
                    
                    b.Property<string>("Action");
                    
                    b.Property<string>("Area");
                    
                    b.Property<string>("Controller");
                    
                    b.Property<DateTime>("CreationDate");
                    
                    b.Key("Id");
                });
            
            builder.Entity("MvcTemplate.Objects.Role", b =>
                {
                    b.Property<string>("Id");
                    
                    b.Property<DateTime>("CreationDate");
                    
                    b.Property<string>("Title");
                    
                    b.Key("Id");
                });
            
            builder.Entity("MvcTemplate.Objects.RolePrivilege", b =>
                {
                    b.Property<string>("Id");
                    
                    b.Property<DateTime>("CreationDate");
                    
                    b.Property<string>("PrivilegeId");
                    
                    b.Property<string>("RoleId");
                    
                    b.Key("Id");
                });
            
            builder.Entity("MvcTemplate.Objects.Account", b =>
                {
                    b.Reference("MvcTemplate.Objects.Role")
                        .InverseCollection()
                        .ForeignKey("RoleId");
                });
            
            builder.Entity("MvcTemplate.Objects.RolePrivilege", b =>
                {
                    b.Reference("MvcTemplate.Objects.Privilege")
                        .InverseCollection()
                        .ForeignKey("PrivilegeId");
                    
                    b.Reference("MvcTemplate.Objects.Role")
                        .InverseCollection()
                        .ForeignKey("RoleId");
                });
        }
    }
}
