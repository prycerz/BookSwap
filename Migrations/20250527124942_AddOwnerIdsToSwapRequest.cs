using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdsToSwapRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$UDV0qZUARMtf6fTbkkCJd.Yu1vJ2ObOUcSpthDj1k3jXh3j8KNQIS", "279fb033-3e23-4158-983b-3a4ac3c287a6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$3Rh4esX94YnDkTAReGSLQOA8k0cVQCiVhDLr/Ah7fj4khHqZIx1pi", "d6b114d3-e822-4d7e-bae3-6921cde879eb" });
        }
    }
}
