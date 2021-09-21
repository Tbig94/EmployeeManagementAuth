using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeManagementAuth.Migrations
{
    public partial class LoggingModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogType",
                table: "LoggingModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogType",
                table: "LoggingModels");
        }
    }
}
