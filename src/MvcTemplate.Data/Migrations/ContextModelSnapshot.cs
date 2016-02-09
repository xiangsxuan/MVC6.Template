using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using MvcTemplate.Data.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MvcTemplate.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("MvcTemplate.Objects.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("IsLocked");

                    b.Property<string>("Passhash")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("RecoveryToken")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime?>("RecoveryTokenExpirationDate");

                    b.Property<string>("RoleId")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 32);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.AuditLog", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("AccountId")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 16);

                    b.Property<string>("Changes")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Log", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("AccountId")
                        .HasAnnotation("MaxLength", 64);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Permission", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("Area")
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("Controller")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<DateTime>("CreationDate");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.RolePermission", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("PermissionId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Account", b =>
                {
                    b.HasOne("MvcTemplate.Objects.Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("MvcTemplate.Objects.RolePermission", b =>
                {
                    b.HasOne("MvcTemplate.Objects.Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId");

                    b.HasOne("MvcTemplate.Objects.Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });
        }
    }
}
