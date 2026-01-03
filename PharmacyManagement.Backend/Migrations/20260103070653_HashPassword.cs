using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyManagement.Backend.Migrations
{
    /// <inheritdoc />
    public partial class HashPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2028, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2028, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2028, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2028, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2028, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2026, 1, 2, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154) });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "CreatedDate", "PasswordAccount" },
                values: new object[] { new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), "$2a$11$djh/SV1pjr7q0R0k0v5/W.L2X9pQ5nJ8q3yH5v5yV5yZ0.ZvZvZvZ" });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "CreatedDate", "PasswordAccount" },
                values: new object[] { new DateTime(2026, 1, 3, 14, 6, 53, 327, DateTimeKind.Local).AddTicks(7154), "$2a$11$djh/SV1pjr7q0R0k0v5/W.L2X9pQ5nJ8q3yH5v5yV5yZ0.ZvZvZvZ" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2028, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2028, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2028, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2028, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2028, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2026, 1, 1, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2026, 1, 1, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "CreatedDate", "PasswordAccount" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), "123" });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "CreatedDate", "PasswordAccount" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 56, 17, 566, DateTimeKind.Local).AddTicks(4350), "123" });
        }
    }
}
