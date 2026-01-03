using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyManagement.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: true);

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
                columns: new[] { "Address", "AvatarUrl", "CreatedDate", "FullName", "PhoneNumber" },
                values: new object[] { null, null, new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), null, null });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "AvatarUrl", "CreatedDate", "FullName", "PhoneNumber" },
                values: new object[] { null, null, new DateTime(2026, 1, 3, 16, 52, 31, 783, DateTimeKind.Local).AddTicks(6378), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "UserAccounts");

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
