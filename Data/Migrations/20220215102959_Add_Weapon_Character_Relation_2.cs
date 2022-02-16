using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game.Data.Migrations
{
    public partial class Add_Weapon_Character_Relation_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Weapon_WeaponId",
                table: "Characters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weapon",
                table: "Weapon");

            migrationBuilder.RenameTable(
                name: "Weapon",
                newName: "Weapons");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weapons",
                table: "Weapons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Weapons_WeaponId",
                table: "Characters",
                column: "WeaponId",
                principalTable: "Weapons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Weapons_WeaponId",
                table: "Characters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weapons",
                table: "Weapons");

            migrationBuilder.RenameTable(
                name: "Weapons",
                newName: "Weapon");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weapon",
                table: "Weapon",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Weapon_WeaponId",
                table: "Characters",
                column: "WeaponId",
                principalTable: "Weapon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
