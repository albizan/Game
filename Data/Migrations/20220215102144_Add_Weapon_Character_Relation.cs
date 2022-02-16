using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game.Data.Migrations
{
    public partial class Add_Weapon_Character_Relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeaponId",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Weapon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Damage = table.Column<int>(type: "int", nullable: false),
                    WeaponType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapon", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_WeaponId",
                table: "Characters",
                column: "WeaponId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Weapon_WeaponId",
                table: "Characters",
                column: "WeaponId",
                principalTable: "Weapon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Weapon_WeaponId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "Weapon");

            migrationBuilder.DropIndex(
                name: "IX_Characters_WeaponId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WeaponId",
                table: "Characters");
        }
    }
}
