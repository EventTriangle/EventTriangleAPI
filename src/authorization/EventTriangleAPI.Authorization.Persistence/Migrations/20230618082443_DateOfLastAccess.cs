using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTriangleAPI.Authorization.Persistence.Migrations
{
    public partial class DateOfLastAccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOfLastAccess",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfLastAccess",
                table: "UserSessions");
        }
    }
}
