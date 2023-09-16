using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppSap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.CreateTable(
                name: "Saps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Firstname = table.Column<string>(type: "TEXT", nullable: true),
                    Middlename = table.Column<string>(type: "TEXT", nullable: true),
                    Lastname = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    BirthPlaceBrgy = table.Column<string>(type: "TEXT", nullable: true),
                    BirthPlaceCity = table.Column<string>(type: "TEXT", nullable: true),
                    Street = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    Tribe = table.Column<string>(type: "TEXT", nullable: true),
                    FatherName = table.Column<string>(type: "TEXT", nullable: true),
                    MotherName = table.Column<string>(type: "TEXT", nullable: true),
                    ParentOccupation = table.Column<string>(type: "TEXT", nullable: true),
                    NumberOfSibling = table.Column<int>(type: "INTEGER", nullable: false),
                    YearSapStarted = table.Column<string>(type: "TEXT", nullable: true),
                    GradeLevel = table.Column<string>(type: "TEXT", nullable: true),
                    SchoolName = table.Column<string>(type: "TEXT", nullable: true),
                    ChurchName = table.Column<string>(type: "TEXT", nullable: true),
                    CommunityInvolvement = table.Column<string>(type: "TEXT", nullable: true),
                    Talent = table.Column<string>(type: "TEXT", nullable: true),
                    Sport = table.Column<string>(type: "TEXT", nullable: true),
                    Ambition = table.Column<string>(type: "TEXT", nullable: true),
                    Motto = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saps", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.DropTable(
                name: "Saps");
        }
    }
}
