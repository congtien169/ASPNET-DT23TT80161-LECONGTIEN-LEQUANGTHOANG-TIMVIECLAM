using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalApp.Migrations
{
    /// <inheritdoc />
    public partial class Add_CVUrl_To_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CVUrl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVUrl",
                table: "Users");
        }
    }
}
