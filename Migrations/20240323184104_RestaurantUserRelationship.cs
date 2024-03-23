using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qr_Menu_API.Migrations
{
    /// <inheritdoc />
    public partial class RestaurantUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantUsers_AspNetUsers_UserId",
                table: "RestaurantUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantUsers_Restaurants_RestaurantId",
                table: "RestaurantUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantUsers_AspNetUsers_UserId",
                table: "RestaurantUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantUsers_Restaurants_RestaurantId",
                table: "RestaurantUsers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantUsers_AspNetUsers_UserId",
                table: "RestaurantUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantUsers_Restaurants_RestaurantId",
                table: "RestaurantUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantUsers_AspNetUsers_UserId",
                table: "RestaurantUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantUsers_Restaurants_RestaurantId",
                table: "RestaurantUsers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
