using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veesy.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddITrackToAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Medias");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "UsedSoftwares",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "UsedSoftwares",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "UsedSoftwares",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "UsedSoftwares",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "TmpMedias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "TmpMedias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "TmpMedias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "TmpMedias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "SubscriptionPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "SubscriptionPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "SubscriptionPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "SubscriptionPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "Skills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "Skills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "Sectors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "Sectors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "Sectors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "Sectors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "PortfolioSectors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "PortfolioSectors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "PortfolioSectors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "PortfolioSectors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "PortfolioMedias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "PortfolioMedias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "PortfolioMedias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "PortfolioMedias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "MyUserUsedSoftwares",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "MyUserUsedSoftwares",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "MyUserUsedSoftwares",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "MyUserUsedSoftwares",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "MyUserSkills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "MyUserSkills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "MyUserSkills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "MyUserSkills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "MyUserSectors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "MyUserSectors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "MyUserSectors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "MyUserSectors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "MyUserLanguagesSpoken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "MyUserLanguagesSpoken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "MyUserLanguagesSpoken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "MyUserLanguagesSpoken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "MyUserInfosToShow",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "MyUserInfosToShow",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "MyUserInfosToShow",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "MyUserInfosToShow",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "MyUserCategoriesWork",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "MyUserCategoriesWork",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "MyUserCategoriesWork",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "MyUserCategoriesWork",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "MediaTags",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "MediaTags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "MediaTags",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "MediaTags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "MediaFormats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "MediaFormats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "MediaFormats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "MediaFormats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "MediaCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "MediaCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "MediaCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "MediaCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "LanguagesSpoken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "LanguagesSpoken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "LanguagesSpoken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "LanguagesSpoken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "InfosToShow",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "InfosToShow",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "InfosToShow",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "InfosToShow",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "Formats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "Formats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "Formats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "Formats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "CategoriesWork",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "CategoriesWork",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "CategoriesWork",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "CategoriesWork",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateRecordDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditRecordDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "UsedSoftwares");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "UsedSoftwares");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "UsedSoftwares");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "UsedSoftwares");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "TmpMedias");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "TmpMedias");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "TmpMedias");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "TmpMedias");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "SubscriptionPlans");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "SubscriptionPlans");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "SubscriptionPlans");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "SubscriptionPlans");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "Sectors");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "Sectors");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "Sectors");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "Sectors");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "PortfolioSectors");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "PortfolioSectors");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "PortfolioSectors");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "PortfolioSectors");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "PortfolioMedias");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "PortfolioMedias");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "PortfolioMedias");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "PortfolioMedias");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "MyUserUsedSoftwares");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "MyUserUsedSoftwares");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "MyUserUsedSoftwares");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "MyUserUsedSoftwares");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "MyUserSkills");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "MyUserSkills");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "MyUserSkills");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "MyUserSkills");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "MyUserSectors");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "MyUserSectors");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "MyUserSectors");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "MyUserSectors");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "MyUserLanguagesSpoken");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "MyUserLanguagesSpoken");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "MyUserLanguagesSpoken");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "MyUserLanguagesSpoken");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "MyUserInfosToShow");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "MyUserInfosToShow");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "MyUserInfosToShow");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "MyUserInfosToShow");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "MyUserCategoriesWork");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "MyUserCategoriesWork");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "MyUserCategoriesWork");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "MyUserCategoriesWork");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "MediaTags");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "MediaTags");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "MediaTags");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "MediaTags");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "MediaFormats");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "MediaFormats");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "MediaFormats");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "MediaFormats");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "MediaCategories");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "MediaCategories");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "MediaCategories");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "MediaCategories");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "LanguagesSpoken");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "LanguagesSpoken");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "LanguagesSpoken");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "LanguagesSpoken");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "InfosToShow");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "InfosToShow");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "InfosToShow");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "InfosToShow");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "Formats");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "Formats");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "Formats");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "Formats");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "CategoriesWork");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "CategoriesWork");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "CategoriesWork");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "CategoriesWork");

            migrationBuilder.DropColumn(
                name: "CreateRecordDate",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LastEditRecordDate",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Medias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
