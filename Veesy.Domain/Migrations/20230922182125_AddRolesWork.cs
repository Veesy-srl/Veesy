using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RolesWork",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateRecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditRecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastEditUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesWork", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyUserRolesWork",
                columns: table => new
                {
                    MyUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleWorkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateRecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditRecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastEditUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyUserRolesWork", x => new { x.MyUserId, x.RoleWorkId });
                    table.ForeignKey(
                        name: "FK_MyUserRolesWork_AspNetUsers_MyUserId",
                        column: x => x.MyUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MyUserRolesWork_RolesWork_RoleWorkId",
                        column: x => x.RoleWorkId,
                        principalTable: "RolesWork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyUserRolesWork_RoleWorkId",
                table: "MyUserRolesWork",
                column: "RoleWorkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyUserRolesWork");

            migrationBuilder.DropTable(
                name: "RolesWork");
        }
    }
}
