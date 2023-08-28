using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class FixMyUserSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MyUserSkills",
                table: "MyUserSkills");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "MyUserSkills",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyUserSkills",
                table: "MyUserSkills",
                columns: new[] { "Id", "MyUserId", "SkillId" });

            migrationBuilder.CreateIndex(
                name: "IX_MyUserSkills_MyUserId",
                table: "MyUserSkills",
                column: "MyUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MyUserSkills",
                table: "MyUserSkills");

            migrationBuilder.DropIndex(
                name: "IX_MyUserSkills_MyUserId",
                table: "MyUserSkills");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MyUserSkills");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyUserSkills",
                table: "MyUserSkills",
                columns: new[] { "MyUserId", "SkillId" });
        }
    }
}
