using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Infrastructure.Migrations
{
    public partial class AddCourses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Course",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1L, "Calculus" });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2L, "Chemistry" });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3L, "English" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Course");
        }
    }
}
