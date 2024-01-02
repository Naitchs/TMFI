using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class CanBeNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HrFiles_Certificates_CertId",
                table: "HrFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_HrFiles_Memos_MemoId",
                table: "HrFiles");

            migrationBuilder.AlterColumn<int>(
                name: "MemoId",
                table: "HrFiles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CertId",
                table: "HrFiles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_HrFiles_Certificates_CertId",
                table: "HrFiles",
                column: "CertId",
                principalTable: "Certificates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HrFiles_Memos_MemoId",
                table: "HrFiles",
                column: "MemoId",
                principalTable: "Memos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HrFiles_Certificates_CertId",
                table: "HrFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_HrFiles_Memos_MemoId",
                table: "HrFiles");

            migrationBuilder.AlterColumn<int>(
                name: "MemoId",
                table: "HrFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CertId",
                table: "HrFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HrFiles_Certificates_CertId",
                table: "HrFiles",
                column: "CertId",
                principalTable: "Certificates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HrFiles_Memos_MemoId",
                table: "HrFiles",
                column: "MemoId",
                principalTable: "Memos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
