using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasker.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedImportant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Important",
                table: "Tasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Important",
                table: "Tasks");
        }
    }
}
