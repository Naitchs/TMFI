using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HrFiles_Certificates_CertsId",
                table: "HrFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_HrFiles_Memos_MemosId",
                table: "HrFiles");

            migrationBuilder.DropIndex(
                name: "IX_HrFiles_CertsId",
                table: "HrFiles");

            migrationBuilder.DropIndex(
                name: "IX_HrFiles_MemosId",
                table: "HrFiles");

            migrationBuilder.DropColumn(
                name: "CertsId",
                table: "HrFiles");

            migrationBuilder.DropColumn(
                name: "MemosId",
                table: "HrFiles");

            migrationBuilder.AddColumn<int>(
                name: "CertId",
                table: "HrFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemoId",
                table: "HrFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HrFiles_CertId",
                table: "HrFiles",
                column: "CertId");

            migrationBuilder.CreateIndex(
                name: "IX_HrFiles_MemoId",
                table: "HrFiles",
                column: "MemoId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HrFiles_Certificates_CertId",
                table: "HrFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_HrFiles_Memos_MemoId",
                table: "HrFiles");

            migrationBuilder.DropIndex(
                name: "IX_HrFiles_CertId",
                table: "HrFiles");

            migrationBuilder.DropIndex(
                name: "IX_HrFiles_MemoId",
                table: "HrFiles");

            migrationBuilder.DropColumn(
                name: "CertId",
                table: "HrFiles");

            migrationBuilder.DropColumn(
                name: "MemoId",
                table: "HrFiles");

            migrationBuilder.AddColumn<int>(
                name: "CertsId",
                table: "HrFiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemosId",
                table: "HrFiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HrFiles_CertsId",
                table: "HrFiles",
                column: "CertsId");

            migrationBuilder.CreateIndex(
                name: "IX_HrFiles_MemosId",
                table: "HrFiles",
                column: "MemosId");

            migrationBuilder.AddForeignKey(
                name: "FK_HrFiles_Certificates_CertsId",
                table: "HrFiles",
                column: "CertsId",
                principalTable: "Certificates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HrFiles_Memos_MemosId",
                table: "HrFiles",
                column: "MemosId",
                principalTable: "Memos",
                principalColumn: "Id");
        }
    }
}
