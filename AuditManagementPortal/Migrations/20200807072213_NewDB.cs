using Microsoft.EntityFrameworkCore.Migrations;

namespace Audit_management_portal.Migrations
{
    public partial class NewDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditResponse",
                table: "AuditResponse");

            migrationBuilder.RenameTable(
                name: "AuditResponse",
                newName: "AuditResponse");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditResponse",
                table: "AuditResponse",
                column: "AuditId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditResponse",
                table: "AuditResponse");

            migrationBuilder.RenameTable(
                name: "AuditResponse",
                newName: "AuditResponse");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditResponse",
                table: "AuditResponse",
                column: "AuditId");
        }
    }
}
