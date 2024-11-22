using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddItemsInUserSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ASN",
                table: "UserSecurities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Accuracy",
                table: "UserSecurities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AreaCode",
                table: "UserSecurities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserSecurities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContinentCode",
                table: "UserSecurities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UserSecurities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "UserSecurities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode3",
                table: "UserSecurities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization",
                table: "UserSecurities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "UserSecurities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "UserSecurities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timezone",
                table: "UserSecurities",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ASN",
                table: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "Accuracy",
                table: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "AreaCode",
                table: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "City",
                table: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "ContinentCode",
                table: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "CountryCode3",
                table: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "Organization",
                table: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "UserSecurities");

            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "UserSecurities");
        }
    }
}
