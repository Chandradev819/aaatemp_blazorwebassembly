using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aaatemp_blazorwebassembly.Server.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NavBarMenuItems",
                columns: table => new
                {
                    MenuId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuDisplayName = table.Column<string>(nullable: true),
                    ParentMenuId = table.Column<int>(nullable: false),
                    UserPolicy = table.Column<string>(nullable: true),
                    MenuURL = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ItemOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavBarMenuItems", x => x.MenuId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NavBarMenuItems");
        }
    }
}
