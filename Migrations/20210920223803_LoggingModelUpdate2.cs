using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeManagementAuth.Migrations
{
    public partial class LoggingModelUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObjType",
                table: "LoggingModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjType",
                table: "LoggingModels");
        }
    }
}
