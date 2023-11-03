using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSuffix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Suffix",
                table: "Saps",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Suffix",
                table: "Ips",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Suffix",
                table: "Saps");

            migrationBuilder.DropColumn(
                name: "Suffix",
                table: "Ips");
        }
    }
}
