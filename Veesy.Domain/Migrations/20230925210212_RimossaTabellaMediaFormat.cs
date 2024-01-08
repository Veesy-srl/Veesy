using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RimossaTabellaMediaFormat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioMedias_MediaFormats_MediaFormatId",
                table: "PortfolioMedias");

            migrationBuilder.DropTable(
                name: "MediaFormats");

            migrationBuilder.DropTable(
                name: "Formats");

            migrationBuilder.RenameColumn(
                name: "MediaFormatId",
                table: "PortfolioMedias",
                newName: "MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioMedias_MediaFormatId",
                table: "PortfolioMedias",
                newName: "IX_PortfolioMedias_MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioMedias_Medias_MediaId",
                table: "PortfolioMedias",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioMedias_Medias_MediaId",
                table: "PortfolioMedias");

            migrationBuilder.RenameColumn(
                name: "MediaId",
                table: "PortfolioMedias",
                newName: "MediaFormatId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioMedias_MediaId",
                table: "PortfolioMedias",
                newName: "IX_PortfolioMedias_MediaFormatId");

            migrationBuilder.CreateTable(
                name: "Formats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateRecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    LastEditRecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaFormats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MediaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateRecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastEditRecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFormats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaFormats_Formats_FormatId",
                        column: x => x.FormatId,
                        principalTable: "Formats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaFormats_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFormats_FormatId",
                table: "MediaFormats",
                column: "FormatId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFormats_MediaId",
                table: "MediaFormats",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioMedias_MediaFormats_MediaFormatId",
                table: "PortfolioMedias",
                column: "MediaFormatId",
                principalTable: "MediaFormats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
