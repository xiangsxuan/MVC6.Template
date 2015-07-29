using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;

namespace MvcTemplate.Data.Migrations
{
    public partial class Initial : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column(type: "nvarchar(128)", nullable: false),
                    AccountId = table.Column(type: "nvarchar(128)", nullable: true),
                    Action = table.Column(type: "nvarchar(128)", nullable: false),
                    Changes = table.Column(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column(type: "datetime2", nullable: false),
                    EntityId = table.Column(type: "nvarchar(128)", nullable: false),
                    EntityName = table.Column(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });
            migration.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column(type: "nvarchar(128)", nullable: false),
                    AccountId = table.Column(type: "nvarchar(128)", nullable: true),
                    CreationDate = table.Column(type: "datetime2", nullable: false),
                    Message = table.Column(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });
            migration.CreateTable(
                name: "Privilege",
                columns: table => new
                {
                    Id = table.Column(type: "nvarchar(128)", nullable: false),
                    Action = table.Column(type: "nvarchar(128)", nullable: false),
                    Area = table.Column(type: "nvarchar(128)", nullable: true),
                    Controller = table.Column(type: "nvarchar(128)", nullable: false),
                    CreationDate = table.Column(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privilege", x => x.Id);
                });
            migration.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column(type: "nvarchar(128)", nullable: false),
                    CreationDate = table.Column(type: "datetime2", nullable: false),
                    Title = table.Column(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });
            migration.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column(type: "nvarchar(128)", nullable: false),
                    CreationDate = table.Column(type: "datetime2", nullable: false),
                    Email = table.Column(type: "nvarchar(256)", nullable: false),
                    IsLocked = table.Column(type: "bit", nullable: false),
                    Passhash = table.Column(type: "nvarchar(512)", nullable: false),
                    RecoveryToken = table.Column(type: "nvarchar(128)", nullable: true),
                    RecoveryTokenExpirationDate = table.Column(type: "datetime2", nullable: true),
                    RoleId = table.Column(type: "nvarchar(128)", nullable: true),
                    Username = table.Column(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Role_RoleId",
                        columns: x => x.RoleId,
                        referencedTable: "Role",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "RolePrivilege",
                columns: table => new
                {
                    Id = table.Column(type: "nvarchar(128)", nullable: false),
                    CreationDate = table.Column(type: "datetime2", nullable: false),
                    PrivilegeId = table.Column(type: "nvarchar(128)", nullable: false),
                    RoleId = table.Column(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePrivilege", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePrivilege_Privilege_PrivilegeId",
                        columns: x => x.PrivilegeId,
                        referencedTable: "Privilege",
                        referencedColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RolePrivilege_Role_RoleId",
                        columns: x => x.RoleId,
                        referencedTable: "Role",
                        referencedColumn: "Id");
                });

            migration.CreateIndex(
                name: "IX_Email",
                table: "Account",
                column: "Email",
                unique: true);
            migration.CreateIndex(
                name: "IX_RoleId",
                table: "Account",
                column: "RoleId");
            migration.CreateIndex(
                name: "IX_Username",
                table: "Account",
                column: "Username",
                unique: true);
            migration.CreateIndex(
                name: "IX_Title",
                table: "Role",
                column: "Title",
                unique: true);
            migration.CreateIndex(
                name: "IX_PrivilegeId",
                table: "RolePrivilege",
                column: "PrivilegeId");
            migration.CreateIndex(
                name: "IX_RoleId",
                table: "RolePrivilege",
                column: "RoleId");
        }

        public override void Down(MigrationBuilder migration)
        {
            migration.DropTable("Account");
            migration.DropTable("AuditLog");
            migration.DropTable("Log");
            migration.DropTable("RolePrivilege");
            migration.DropTable("Privilege");
            migration.DropTable("Role");
        }
    }
}
