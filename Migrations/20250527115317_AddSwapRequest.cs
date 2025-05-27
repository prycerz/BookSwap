using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class AddSwapRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SwapRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OfferedBookId = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetBookId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwapRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SwapRequests_Books_OfferedBookId",
                        column: x => x.OfferedBookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SwapRequests_Books_TargetBookId",
                        column: x => x.TargetBookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$3Rh4esX94YnDkTAReGSLQOA8k0cVQCiVhDLr/Ah7fj4khHqZIx1pi", "d6b114d3-e822-4d7e-bae3-6921cde879eb" });

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_OfferedBookId",
                table: "SwapRequests",
                column: "OfferedBookId");

            migrationBuilder.CreateIndex(
                name: "IX_SwapRequests_TargetBookId",
                table: "SwapRequests",
                column: "TargetBookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SwapRequests");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Token" },
                values: new object[] { "$2a$11$fZh7RLd6nFZGoPFljx6pq.aNfRcsAtpQ7phwuAODiymmGPVqtKQbm", "ece1b71c-7280-4a21-99e9-642e8f1cece2" });
        }
    }
}
