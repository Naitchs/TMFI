using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Saps",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Ips",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "AppDocumentations",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Saps");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Ips");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "AppDocumentations");
        }
    }
}
