using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game.Migrations
{
    public partial class CreatePerksToWeaponsAndCharacters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Perk",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Increment = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterPerk",
                columns: table => new
                {
                    CharactersId = table.Column<int>(type: "int", nullable: false),
                    PerksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterPerk", x => new { x.CharactersId, x.PerksId });
                    table.ForeignKey(
                        name: "FK_CharacterPerk_Characters_CharactersId",
                        column: x => x.CharactersId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterPerk_Perk_PerksId",
                        column: x => x.PerksId,
                        principalTable: "Perk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerkWeapon",
                columns: table => new
                {
                    PerksId = table.Column<int>(type: "int", nullable: false),
                    WeaponsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerkWeapon", x => new { x.PerksId, x.WeaponsId });
                    table.ForeignKey(
                        name: "FK_PerkWeapon_Perk_PerksId",
                        column: x => x.PerksId,
                        principalTable: "Perk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerkWeapon_Weapons_WeaponsId",
                        column: x => x.WeaponsId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPerk_PerksId",
                table: "CharacterPerk",
                column: "PerksId");

            migrationBuilder.CreateIndex(
                name: "IX_PerkWeapon_WeaponsId",
                table: "PerkWeapon",
                column: "WeaponsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterPerk");

            migrationBuilder.DropTable(
                name: "PerkWeapon");

            migrationBuilder.DropTable(
                name: "Perk");
        }
    }
}
