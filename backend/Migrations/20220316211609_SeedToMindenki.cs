using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webbot.Migrations
{
    public partial class SeedToMindenki : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO GroupPlayer SELECT 1 as GroupsId, Id as PlayersId FROM Players");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM GroupPlayer");
        }
    }
}
