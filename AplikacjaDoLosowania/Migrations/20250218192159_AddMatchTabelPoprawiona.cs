using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplikacjaDoLosowania.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchTabelPoprawiona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Team2Plyers",
                table: "Matches",
                newName: "Team2Players");

            migrationBuilder.RenameColumn(
                name: "Team1Plyers",
                table: "Matches",
                newName: "Team1Players");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Team2Players",
                table: "Matches",
                newName: "Team2Plyers");

            migrationBuilder.RenameColumn(
                name: "Team1Players",
                table: "Matches",
                newName: "Team1Plyers");
        }
    }
}
