using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessageRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$/S95Rp60ZvLuwlbJqHqsmecz4W3bcgFmy5nWdp9eihgDOmd8dpM/q", "cad78774-c820-4069-82d3-fd4db22efd23" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$/FtAzT1g8p3ZRrHW16sGb.SSKB2cnu1dkpjXv1VG2aGwsPrUoU2fG", "fd95068d-df8d-4247-9d31-910c55a98c69" });
        }
    }
}
