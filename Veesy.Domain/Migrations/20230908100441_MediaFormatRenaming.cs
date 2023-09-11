using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class MediaFormatRenaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "MediaFormats");

            migrationBuilder.RenameColumn(
                name: "ProcessedFileName",
                table: "MediaFormats",
                newName: "FileName");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "MediaFormats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "MediaFormats");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "MediaFormats",
                newName: "ProcessedFileName");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "MediaFormats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
