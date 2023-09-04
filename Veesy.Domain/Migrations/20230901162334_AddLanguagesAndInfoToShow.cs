using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguagesAndInfoToShow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfosToShow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfosToShow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguagesSpoken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguagesSpoken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyUserInfosToShow",
                columns: table => new
                {
                    MyUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InfoToShowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Show = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyUserInfosToShow", x => new { x.MyUserId, x.InfoToShowId });
                    table.ForeignKey(
                        name: "FK_MyUserInfosToShow_AspNetUsers_MyUserId",
                        column: x => x.MyUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MyUserInfosToShow_InfosToShow_InfoToShowId",
                        column: x => x.InfoToShowId,
                        principalTable: "InfosToShow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyUserLanguagesSpoken",
                columns: table => new
                {
                    MyUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LanguageSpokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyUserLanguagesSpoken", x => new { x.MyUserId, x.LanguageSpokenId });
                    table.ForeignKey(
                        name: "FK_MyUserLanguagesSpoken_AspNetUsers_MyUserId",
                        column: x => x.MyUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MyUserLanguagesSpoken_LanguagesSpoken_LanguageSpokenId",
                        column: x => x.LanguageSpokenId,
                        principalTable: "LanguagesSpoken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyUserInfosToShow_InfoToShowId",
                table: "MyUserInfosToShow",
                column: "InfoToShowId");

            migrationBuilder.CreateIndex(
                name: "IX_MyUserLanguagesSpoken_LanguageSpokenId",
                table: "MyUserLanguagesSpoken",
                column: "LanguageSpokenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyUserInfosToShow");

            migrationBuilder.DropTable(
                name: "MyUserLanguagesSpoken");

            migrationBuilder.DropTable(
                name: "InfosToShow");

            migrationBuilder.DropTable(
                name: "LanguagesSpoken");
        }
    }
}
