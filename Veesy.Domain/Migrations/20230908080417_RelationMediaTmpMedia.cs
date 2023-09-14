using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RelationMediaTmpMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProcessedName",
                table: "Medias",
                newName: "OriginalFileName");

            migrationBuilder.AddColumn<Guid>(
                name: "MediaId",
                table: "TmpMedias",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TmpMedias_MediaId",
                table: "TmpMedias",
                column: "MediaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TmpMedias_Medias_MediaId",
                table: "TmpMedias",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TmpMedias_Medias_MediaId",
                table: "TmpMedias");

            migrationBuilder.DropIndex(
                name: "IX_TmpMedias_MediaId",
                table: "TmpMedias");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "TmpMedias");

            migrationBuilder.RenameColumn(
                name: "OriginalFileName",
                table: "Medias",
                newName: "ProcessedName");
        }
    }
}
