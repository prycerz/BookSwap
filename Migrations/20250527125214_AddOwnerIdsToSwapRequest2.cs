using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdsToSwapRequest2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfferedBookOwnerId",
                table: "SwapRequests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetBookOwnerId",
                table: "SwapRequests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$PHMpxiBUeBp9Z1RT2s6YJOlqKsIh4.Tv5jBwcFOJSyjCbViuaLl2e", "6efd0588-d267-4718-b82c-d91b2f4d5dc6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferedBookOwnerId",
                table: "SwapRequests");

            migrationBuilder.DropColumn(
                name: "TargetBookOwnerId",
                table: "SwapRequests");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$UDV0qZUARMtf6fTbkkCJd.Yu1vJ2ObOUcSpthDj1k3jXh3j8KNQIS", "279fb033-3e23-4158-983b-3a4ac3c287a6" });
        }
    }
}
