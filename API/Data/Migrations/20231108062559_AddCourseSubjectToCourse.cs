using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseSubjectToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubjects_Courses_CourseId",
                table: "CourseSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubjects_Subjects_SubjectId",
                table: "CourseSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSubjects",
                table: "CourseSubjects");

            migrationBuilder.RenameTable(
                name: "CourseSubjects",
                newName: "CourseSubject");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSubjects_SubjectId",
                table: "CourseSubject",
                newName: "IX_CourseSubject_SubjectId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CourseSubject",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSubject",
                table: "CourseSubject",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppCourseAppSubject",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCourseAppSubject", x => new { x.CoursesId, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_AppCourseAppSubject_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppCourseAppSubject_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubject_CourseId",
                table: "CourseSubject",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCourseAppSubject_SubjectsId",
                table: "AppCourseAppSubject",
                column: "SubjectsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubject_Courses_CourseId",
                table: "CourseSubject",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubject_Subjects_SubjectId",
                table: "CourseSubject",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubject_Courses_CourseId",
                table: "CourseSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubject_Subjects_SubjectId",
                table: "CourseSubject");

            migrationBuilder.DropTable(
                name: "AppCourseAppSubject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSubject",
                table: "CourseSubject");

            migrationBuilder.DropIndex(
                name: "IX_CourseSubject_CourseId",
                table: "CourseSubject");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CourseSubject");

            migrationBuilder.RenameTable(
                name: "CourseSubject",
                newName: "CourseSubjects");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSubject_SubjectId",
                table: "CourseSubjects",
                newName: "IX_CourseSubjects_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSubjects",
                table: "CourseSubjects",
                columns: new[] { "CourseId", "SubjectId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubjects_Courses_CourseId",
                table: "CourseSubjects",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubjects_Subjects_SubjectId",
                table: "CourseSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
