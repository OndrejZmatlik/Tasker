using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasker.Web.Migrations
{
    /// <inheritdoc />
    public partial class MultipleSubjectsAndDateRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create join table first so we can copy data into it
            migrationBuilder.CreateTable(
                name: "TaskSubjects",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSubjects", x => new { x.SubjectId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_TaskSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskSubjects_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskSubjects_TaskId",
                table: "TaskSubjects",
                column: "TaskId");

            // 2. Copy existing single-subject assignments into the join table
            migrationBuilder.Sql(
                @"INSERT INTO ""TaskSubjects"" (""TaskId"", ""SubjectId"")
                  SELECT ""Id"", ""SubjectId""
                  FROM ""Tasks""
                  WHERE ""SubjectId"" != '00000000-0000-0000-0000-000000000000'");

            // 3. Now it is safe to remove the old FK and column
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Subjects_SubjectId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SubjectId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Tasks");

            // 4. Rename Deadline → DeadlineTo and add optional DeadlineFrom
            migrationBuilder.RenameColumn(
                name: "Deadline",
                table: "Tasks",
                newName: "DeadlineTo");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DeadlineFrom",
                table: "Tasks",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskSubjects");

            migrationBuilder.DropColumn(
                name: "DeadlineFrom",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "DeadlineTo",
                table: "Tasks",
                newName: "Deadline");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SubjectId",
                table: "Tasks",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Subjects_SubjectId",
                table: "Tasks",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
