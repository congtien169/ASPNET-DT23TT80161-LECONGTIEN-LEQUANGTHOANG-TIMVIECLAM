using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApp.Migrations
{
    /// <inheritdoc />
    public partial class Update_Latest_Model_Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Jobs_JobId1",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_JobId1",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "JobId1",
                table: "Applications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JobId1",
                table: "Applications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobId1",
                table: "Applications",
                column: "JobId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Jobs_JobId1",
                table: "Applications",
                column: "JobId1",
                principalTable: "Jobs",
                principalColumn: "Id");
        }
    }
}
