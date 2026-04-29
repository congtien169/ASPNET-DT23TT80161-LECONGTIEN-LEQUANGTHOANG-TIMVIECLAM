using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApp.Migrations
{
    /// <inheritdoc />
    public partial class Add_Requirements_To_Job : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Requirements",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Requirements",
                table: "Jobs");
        }
    }
}
