using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class AddBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$sRjwOy24uIZuyDaH4/pLvOAN4fsquOFzq5hrd0Ep8q2tAJ5cFY37.", "5b2bf184-0e18-48eb-8695-833826b4c751" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$IH3TLcSu96U89WDNVdjEOewS2uZXCq05LbsWRsvGOsZmwV8Gt3fXi", "65fc0064-64e4-4791-886a-d214f41769d4" });
        }
    }
}
