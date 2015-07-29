using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations.Infrastructure;
using MvcTemplate.Data.Core;

namespace MvcTemplateDataMigrations
{
    [ContextType(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        public override void BuildModel(ModelBuilder builder)
        {
            builder
                .Annotation("ProductVersion", "7.0.0-beta6-13815")
                .Annotation("SqlServer:ValueGenerationStrategy", "IdentityColumn");

            builder.Entity("MvcTemplate.Objects.Account", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Email")
                        .Required()
                        .Annotation("MaxLength", 256);

                    b.Property<bool>("IsLocked");

                    b.Property<string>("Passhash")
                        .Required()
                        .Annotation("MaxLength", 512);

                    b.Property<string>("RecoveryToken")
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime?>("RecoveryTokenExpirationDate");

                    b.Property<string>("RoleId")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Username")
                        .Required()
                        .Annotation("MaxLength", 128);

                    b.Key("Id");
                });

            builder.Entity("MvcTemplate.Objects.AuditLog", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("AccountId")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Action")
                        .Required()
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Changes")
                        .Required();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("EntityId")
                        .Required()
                        .Annotation("MaxLength", 128);

                    b.Property<string>("EntityName")
                        .Required()
                        .Annotation("MaxLength", 128);

                    b.Key("Id");
                });

            builder.Entity("MvcTemplate.Objects.Log", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("AccountId")
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Message")
                        .Required();

                    b.Key("Id");
                });

            builder.Entity("MvcTemplate.Objects.Privilege", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Action")
                        .Required()
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Area")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Controller")
                        .Required()
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Key("Id");
                });

            builder.Entity("MvcTemplate.Objects.Role", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Title")
                        .Required()
                        .Annotation("MaxLength", 128);

                    b.Key("Id");
                });

            builder.Entity("MvcTemplate.Objects.RolePrivilege", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("PrivilegeId")
                        .Required()
                        .Annotation("MaxLength", 128);

                    b.Property<string>("RoleId")
                        .Required()
                        .Annotation("MaxLength", 128);

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
