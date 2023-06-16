using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RentCarsAPI.Migrations
{
    public partial class zmianaWtabelach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableNow",
                table: "Cars",
                newName: "EfficientNow");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDateOfReturn",
                table: "Hires",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedDateOfReturn",
                table: "Hires");

            migrationBuilder.RenameColumn(
                name: "EfficientNow",
                table: "Cars",
                newName: "AvailableNow");
        }
    }
}
