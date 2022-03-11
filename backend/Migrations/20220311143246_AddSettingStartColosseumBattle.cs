using Microsoft.EntityFrameworkCore.Migrations;
using webbot.Consts;

#nullable disable

namespace webbot.Migrations
{
    public partial class AddSettingStartColosseumBattle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO SETTINGS VALUES('{DatabaseConsts.StartColosseumBattleSetting}',0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
