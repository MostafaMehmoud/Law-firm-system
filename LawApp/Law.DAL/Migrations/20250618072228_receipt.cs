using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Law.DAL.Migrations
{
    /// <inheritdoc />
    public partial class receipt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "receipts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    DateReceipt = table.Column<DateOnly>(type: "date", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    ChequeNumber = table.Column<int>(type: "int", nullable: true),
                    ChequeDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignType = table.Column<int>(type: "int", nullable: false),
                    DesignNumber = table.Column<int>(type: "int", nullable: true),
                    ProjectNumber = table.Column<int>(type: "int", nullable: true),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_receipts_Code",
                table: "receipts",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "receipts");
        }
    }
}
