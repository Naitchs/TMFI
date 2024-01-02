using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMemoUploads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemoId",
                table: "HrFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemosId",
                table: "HrFiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Memos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    MemoType = table.Column<string>(type: "TEXT", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HrFiles_MemosId",
                table: "HrFiles",
                column: "MemosId");

            migrationBuilder.AddForeignKey(
                name: "FK_HrFiles_Memos_MemosId",
                table: "HrFiles",
                column: "MemosId",
                principalTable: "Memos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HrFiles_Memos_MemosId",
                table: "HrFiles");

            migrationBuilder.DropTable(
                name: "Memos");

            migrationBuilder.DropIndex(
                name: "IX_HrFiles_MemosId",
                table: "HrFiles");

            migrationBuilder.DropColumn(
                name: "MemoId",
                table: "HrFiles");

            migrationBuilder.DropColumn(
                name: "MemosId",
                table: "HrFiles");
        }
    }
}
