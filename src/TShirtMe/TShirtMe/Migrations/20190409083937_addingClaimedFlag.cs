using Microsoft.EntityFrameworkCore.Migrations;

namespace TShirtMe.Migrations
{
    public partial class addingClaimedFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Claimed",
                table: "Entries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Claimed",
                table: "Entries");
        }
    }
}
