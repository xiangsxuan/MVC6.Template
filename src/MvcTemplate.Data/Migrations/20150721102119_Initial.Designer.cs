using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using MvcTemplate.Data.Core;

namespace MvcTemplate.Data.Migrations
{
    [ContextType(typeof(Context))]
    partial class Initial
    {
        public override string Id
        {
            get { return "20150721102119_Initial"; }
        }
        
        public override string ProductVersion
        {
            get { return "7.0.0-beta5-13549"; }
        }
        
        public override void BuildTargetModel(ModelBuilder builder)
        {
            builder
                .Annotation("SqlServer:DefaultSequenceName", "DefaultSequence")
                .Annotation("SqlServer:Sequence:.DefaultSequence", "'DefaultSequence', '', '1', '10', '', '', 'Int64', 'False'")
                .Annotation("SqlServer:ValueGeneration", "Sequence");
            
            builder.Entity("MvcTemplate.Objects.Account", b =>
                {
                    b.Property<string>("Id")
                        .GenerateValueOnAdd();
                    
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
                    b.Property<string>("Id")
                        .GenerateValueOnAdd();
                    
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
                    b.Property<string>("Id")
                        .GenerateValueOnAdd();
                    
                    b.Property<string>("AccountId");
                    
                    b.Property<DateTime>("CreationDate");
                    
                    b.Property<string>("Message");
                    
                    b.Key("Id");
                });
            
            builder.Entity("MvcTemplate.Objects.Privilege", b =>
                {
                    b.Property<string>("Id")
                        .GenerateValueOnAdd();
                    
                    b.Property<string>("Action");
                    
                    b.Property<string>("Area");
                    
                    b.Property<string>("Controller");
                    
                    b.Property<DateTime>("CreationDate");
                    
                    b.Key("Id");
                });
            
            builder.Entity("MvcTemplate.Objects.Role", b =>
                {
                    b.Property<string>("Id")
                        .GenerateValueOnAdd();
                    
                    b.Property<DateTime>("CreationDate");
                    
                    b.Property<string>("Title");
                    
                    b.Key("Id");
                });
            
            builder.Entity("MvcTemplate.Objects.RolePrivilege", b =>
                {
                    b.Property<string>("Id")
                        .GenerateValueOnAdd();
                    
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
