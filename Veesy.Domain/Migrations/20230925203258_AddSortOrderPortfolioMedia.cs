﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddSortOrderPortfolioMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "PortfolioMedias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "PortfolioMedias");
        }
    }
}