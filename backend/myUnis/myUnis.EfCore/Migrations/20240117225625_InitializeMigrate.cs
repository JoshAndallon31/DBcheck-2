using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusFeedApi.Migrations
{
    /// <inheritdoc />
    public partial class InitializeMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampusFeedInfo",
                columns: table => new
                {
                    CampusFeedId = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Like = table.Column<int>(type: "INTEGER", nullable: false),
                    Dislike = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusFeedInfo", x => x.CampusFeedId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampusFeedInfo");
        }
    }
}
