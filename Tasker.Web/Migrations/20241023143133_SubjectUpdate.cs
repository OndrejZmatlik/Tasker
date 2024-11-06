using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasker.Web.Migrations
{
    /// <inheritdoc />
    public partial class SubjectUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tasks",
                newName: "ShortName");

           

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Subjects",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TaskTypes",
                newName: "ShortName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tasks",
                newName: "ShortName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Groups",
                newName: "ShortName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUserTokens",
                newName: "ShortName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetRoles",
                newName: "ShortName");
        }
    }
}
