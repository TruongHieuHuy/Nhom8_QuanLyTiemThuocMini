using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyManagement.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatarUrlToEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AvatarUrl", "CreatedDate", "LastModifiedDate", "StartDate" },
                values: new object[] { null, new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2028, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2028, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2028, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2028, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2028, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2026, 1, 2, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2026, 1, 2, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300), new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300) });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300));

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 3, 17, 21, 7, 608, DateTimeKind.Local).AddTicks(1300));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2028, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2028, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2028, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2028, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ExpiryDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2028, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2026, 1, 2, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2026, 1, 2, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "OrderDate" },
                values: new object[] { new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378));

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378));
        }
    }
}
