using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using MvcTemplate.Data.Core;

namespace MvcTemplate.Data.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20150721102119_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta8-15964")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MvcTemplate.Objects.Account", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .Annotation("MaxLength", 256);

                    b.Property<bool>("IsLocked");

                    b.Property<string>("Passhash")
                        .IsRequired()
                        .Annotation("MaxLength", 512);

                    b.Property<string>("RecoveryToken")
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime?>("RecoveryTokenExpirationDate");

                    b.Property<string>("RoleId")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Username")
                        .IsRequired()
                        .Annotation("MaxLength", 128);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.AuditLog", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("AccountId")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Action")
                        .IsRequired()
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Changes")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .Annotation("MaxLength", 128);

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .Annotation("MaxLength", 128);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Log", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("AccountId")
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Privilege", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Action")
                        .IsRequired()
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Area")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Controller")
                        .IsRequired()
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Role", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .Annotation("MaxLength", 128);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.RolePrivilege", b =>
                {
                    b.Property<string>("Id")
                        .Annotation("MaxLength", 128);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("PrivilegeId")
                        .IsRequired()
                        .Annotation("MaxLength", 128);

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .Annotation("MaxLength", 128);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Account", b =>
                {
                    b.HasOne("MvcTemplate.Objects.Role")
                        .WithMany()
                        .ForeignKey("RoleId");
                });

            modelBuilder.Entity("MvcTemplate.Objects.RolePrivilege", b =>
                {
                    b.HasOne("MvcTemplate.Objects.Privilege")
                        .WithMany()
                        .ForeignKey("PrivilegeId");

                    b.HasOne("MvcTemplate.Objects.Role")
                        .WithMany()
                        .ForeignKey("RoleId");
                });
        }
    }
}
