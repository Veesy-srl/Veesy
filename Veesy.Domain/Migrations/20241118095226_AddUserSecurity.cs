using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSecurities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MyUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LastAccess = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SessionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecurities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSecurities_AspNetUsers_MyUserId",
                        column: x => x.MyUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSecurities_MyUserId",
                table: "UserSecurities",
                column: "MyUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSecurities");
        }
    }
}
