using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HrFiles_Certificates_CertId",
                table: "HrFiles");

            migrationBuilder.AddForeignKey(
                name: "FK_HrFiles_Certificates_CertId",
                table: "HrFiles",
                column: "CertId",
                principalTable: "Certificates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HrFiles_Certificates_CertId",
                table: "HrFiles");

            migrationBuilder.AddForeignKey(
                name: "FK_HrFiles_Certificates_CertId",
                table: "HrFiles",
                column: "CertId",
                principalTable: "Certificates",
                principalColumn: "Id");
        }
    }
}
