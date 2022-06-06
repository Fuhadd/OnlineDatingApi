using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineDatingApi.Data.Migrations
{
    public partial class AddLookingFor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LookingFor",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LookingFor",
                table: "AspNetUsers");
        }
    }
}
