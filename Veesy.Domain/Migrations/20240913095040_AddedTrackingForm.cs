using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddedTrackingForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrackingForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailSender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recipient = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormType = table.Column<int>(type: "int", nullable: false),
                    CreateRecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditRecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastEditUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingForms", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackingForms");
        }
    }
}
