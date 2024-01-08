using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class MyUserSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyUserSkill_AspNetUsers_MyUserId",
                table: "MyUserSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyUserSkill",
                table: "MyUserSkill");

            migrationBuilder.DropIndex(
                name: "IX_MyUserSkill_MyUserId",
                table: "MyUserSkill");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MyUserSkill");

            migrationBuilder.RenameTable(
                name: "MyUserSkill",
                newName: "MyUserSkills");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MyUserSkills",
                newName: "SkillId");

            migrationBuilder.AlterColumn<string>(
                name: "MyUserId",
                table: "MyUserSkills",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrincipal",
                table: "MyUserSkills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyUserSkills",
                table: "MyUserSkills",
                columns: new[] { "MyUserId", "SkillId" });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyUserSkills_SkillId",
                table: "MyUserSkills",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyUserSkills_AspNetUsers_MyUserId",
                table: "MyUserSkills",
                column: "MyUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MyUserSkills_Skills_SkillId",
                table: "MyUserSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyUserSkills_AspNetUsers_MyUserId",
                table: "MyUserSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_MyUserSkills_Skills_SkillId",
                table: "MyUserSkills");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyUserSkills",
                table: "MyUserSkills");

            migrationBuilder.DropIndex(
                name: "IX_MyUserSkills_SkillId",
                table: "MyUserSkills");

            migrationBuilder.DropColumn(
                name: "IsPrincipal",
                table: "MyUserSkills");

            migrationBuilder.RenameTable(
                name: "MyUserSkills",
                newName: "MyUserSkill");

            migrationBuilder.RenameColumn(
                name: "SkillId",
                table: "MyUserSkill",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "MyUserId",
                table: "MyUserSkill",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MyUserSkill",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyUserSkill",
                table: "MyUserSkill",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MyUserSkill_MyUserId",
                table: "MyUserSkill",
                column: "MyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyUserSkill_AspNetUsers_MyUserId",
                table: "MyUserSkill",
                column: "MyUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
