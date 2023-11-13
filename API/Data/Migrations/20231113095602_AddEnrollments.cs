using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrollments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubject_Courses_CourseId",
                table: "CourseSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubject_Subjects_SubjectId",
                table: "CourseSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_Ips_Enrolls_AppEnrollId",
                table: "Ips");

            migrationBuilder.DropTable(
                name: "AppCourseAppSubject");

            migrationBuilder.DropTable(
                name: "Enrolls");

            migrationBuilder.DropIndex(
                name: "IX_Ips_AppEnrollId",
                table: "Ips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSubject",
                table: "CourseSubject");

            migrationBuilder.DropIndex(
                name: "IX_CourseSubject_CourseId",
                table: "CourseSubject");

            migrationBuilder.DropColumn(
                name: "AppEnrollId",
                table: "Ips");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CourseSubject");

            migrationBuilder.RenameTable(
                name: "CourseSubject",
                newName: "CourseSubjects");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "CourseSubjects",
                newName: "SubjectID");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CourseSubjects",
                newName: "CourseID");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSubject_SubjectId",
                table: "CourseSubjects",
                newName: "IX_CourseSubjects_SubjectID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSubjects",
                table: "CourseSubjects",
                columns: new[] { "CourseID", "SubjectID" });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentID = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectID = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceID);
                    table.ForeignKey(
                        name: "FK_Attendances_Ips_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Ips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseStudents",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudents", x => new { x.CourseID, x.StudentID });
                    table.ForeignKey(
                        name: "FK_CourseStudents_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStudents_Ips_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Ips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentID",
                table: "Attendances",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SubjectID",
                table: "Attendances",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudents_StudentID",
                table: "CourseStudents",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubjects_Courses_CourseID",
                table: "CourseSubjects",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSubjects_Subjects_SubjectID",
                table: "CourseSubjects",
                column: "SubjectID",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubjects_Courses_CourseID",
                table: "CourseSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSubjects_Subjects_SubjectID",
                table: "CourseSubjects");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "CourseStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSubjects",
                table: "CourseSubjects");

            migrationBuilder.RenameTable(
                name: "CourseSubjects",
                newName: "CourseSubject");

            migrationBuilder.RenameColumn(
                name: "SubjectID",
                table: "CourseSubject",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "CourseSubject",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSubjects_SubjectID",
                table: "CourseSubject",
                newName: "IX_CourseSubject_SubjectId");

            migrationBuilder.AddColumn<int>(
                name: "AppEnrollId",
                table: "Ips",
                type: "INTEGER",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "Enrolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrolls_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ips_AppEnrollId",
                table: "Ips",
                column: "AppEnrollId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubject_CourseId",
                table: "CourseSubject",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCourseAppSubject_SubjectsId",
                table: "AppCourseAppSubject",
                column: "SubjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrolls_CourseId",
                table: "Enrolls",
                column: "CourseId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Ips_Enrolls_AppEnrollId",
                table: "Ips",
                column: "AppEnrollId",
                principalTable: "Enrolls",
                principalColumn: "Id");
        }
    }
}
