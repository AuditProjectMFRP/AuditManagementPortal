﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Audit_management_portal.Migrations
{
    public partial class NewMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditResponse",
                columns: table => new
                {
                    AuditId = table.Column<int>(nullable: false),
                    ProjectExecutionStatus = table.Column<string>(nullable: true),
                    RemedialActionDuration = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditResponse", x => x.AuditId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditResponse");
        }
    }
}
