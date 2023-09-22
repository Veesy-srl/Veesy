using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddPrefixPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberPrefix",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumberPrefix",
                table: "AspNetUsers");
        }
    }
}
