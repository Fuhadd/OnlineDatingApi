using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineDatingApi.Data.Migrations
{
    public partial class UpdateUserTableWithImageUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserImagesUrl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ApiUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImagesUrl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserImagesUrl_AspNetUsers_ApiUserId",
                        column: x => x.ApiUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserImagesUrl_ApiUserId",
                table: "UserImagesUrl",
                column: "ApiUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserImagesUrl");
        }
    }
}
