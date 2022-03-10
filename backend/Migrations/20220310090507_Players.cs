using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webbot.Migrations
{
    public partial class Players : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    EmailPassword = table.Column<string>(type: "TEXT", nullable: true),
                    PlayerCode = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    ArmyCode = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Cookie = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
