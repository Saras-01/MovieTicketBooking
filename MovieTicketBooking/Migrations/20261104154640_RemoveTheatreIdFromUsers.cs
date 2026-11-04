using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTheatreIdFromUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Theatres_TheatreId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TheatreId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TheatreId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TheatreId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TheatreId",
                table: "Users",
                column: "TheatreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Theatres_TheatreId",
                table: "Users",
                column: "TheatreId",
                principalTable: "Theatres",
                principalColumn: "TheatreId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
