using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasker.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Important",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Tasks");

            migrationBuilder.AddColumn<bool>(
                name: "Important",
                table: "Tasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
