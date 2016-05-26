using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MvcTemplate.Data.Core;

namespace MvcTemplate.Data.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20160522062228_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20901")
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

                    b.HasIndex("RoleId");

                    b.ToTable("Account");
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

                    b.ToTable("Permission");
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

                    b.ToTable("Role");
                });

            modelBuilder.Entity("MvcTemplate.Objects.RolePermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("PermissionId");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermission");
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
