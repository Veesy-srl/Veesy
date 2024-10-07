using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class MyUserInTrackingForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "TrackingForms");

            migrationBuilder.AddColumn<string>(
                name: "MyUserId",
                table: "TrackingForms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingForms_MyUserId",
                table: "TrackingForms",
                column: "MyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingForms_AspNetUsers_MyUserId",
                table: "TrackingForms",
                column: "MyUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingForms_AspNetUsers_MyUserId",
                table: "TrackingForms");

            migrationBuilder.DropIndex(
                name: "IX_TrackingForms_MyUserId",
                table: "TrackingForms");

            migrationBuilder.DropColumn(
                name: "MyUserId",
                table: "TrackingForms");

            migrationBuilder.AddColumn<string>(
                name: "RecipientId",
                table: "TrackingForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
