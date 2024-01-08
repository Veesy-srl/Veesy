using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriesWork",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesWork", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyUserCategoriesWork",
                columns: table => new
                {
                    MyUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryWorkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyUserCategoriesWork", x => new { x.MyUserId, x.CategoryWorkId });
                    table.ForeignKey(
                        name: "FK_MyUserCategoriesWork_AspNetUsers_MyUserId",
                        column: x => x.MyUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MyUserCategoriesWork_CategoriesWork_CategoryWorkId",
                        column: x => x.CategoryWorkId,
                        principalTable: "CategoriesWork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyUserCategoriesWork_CategoryWorkId",
                table: "MyUserCategoriesWork",
                column: "CategoryWorkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyUserCategoriesWork");

            migrationBuilder.DropTable(
                name: "CategoriesWork");
        }
    }
}
