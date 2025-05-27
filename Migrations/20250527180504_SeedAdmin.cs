using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$lhv99SA1eoELrlMYPglcX.xXw7Y2EKbe5ReIbHGjBP7EZAMuR9Exe", "154af95e-7047-4768-b4ef-3c0a7e9f5665" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$PHMpxiBUeBp9Z1RT2s6YJOlqKsIh4.Tv5jBwcFOJSyjCbViuaLl2e", "6efd0588-d267-4718-b82c-d91b2f4d5dc6" });
        }
    }
}
