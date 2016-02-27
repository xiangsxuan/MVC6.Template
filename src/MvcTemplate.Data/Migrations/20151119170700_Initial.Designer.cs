using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using MvcTemplate.Data.Core;

namespace MvcTemplate.Data.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20151119170700_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MvcTemplate.Objects.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

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

                    b.Property<int?>("RoleId");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 32);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AccountId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.Permission", b =>
                {
                    b.Property<int>("Id");

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
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MvcTemplate.Objects.RolePermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("PermissionId");

                    b.Property<int>("RoleId");

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
