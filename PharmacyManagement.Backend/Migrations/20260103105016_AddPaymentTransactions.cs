using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyManagement.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TxnRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2028, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2028, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2028, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2028, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2028, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2026, 1, 2, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 1, 2, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83) });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83));

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 3, 17, 50, 16, 171, DateTimeKind.Local).AddTicks(83));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2028, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2028, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2028, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2028, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2028, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2025, 12, 31, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2025, 12, 31, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584), new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584));

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 1, 19, 52, 55, 933, DateTimeKind.Local).AddTicks(4584));
        }
    }
}
