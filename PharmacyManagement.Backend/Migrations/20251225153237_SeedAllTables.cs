using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PharmacyManagement.Backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalSpending = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicineGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PromotionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinOrderAmount = table.Column<int>(type: "int", nullable: false),
                    TargetCustomer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Debt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicineGroupId = table.Column<int>(type: "int", nullable: false),
                    Usage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentStock = table.Column<int>(type: "int", nullable: false),
                    MinStockLevel = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicines_MedicineGroups_MedicineGroupId",
                        column: x => x.MedicineGroupId,
                        principalTable: "MedicineGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StockBefore = table.Column<int>(type: "int", nullable: false),
                    StockAfter = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryHistories_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicinePromotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    PromotionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinePromotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicinePromotions_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicinePromotions_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    WorkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrdersProcessed = table.Column<int>(type: "int", nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkHistories_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "City", "CreatedDate", "DateOfBirth", "District", "Email", "Gender", "IsActive", "LastModifiedDate", "Name", "PhoneNumber", "TotalSpending", "Ward" },
                values: new object[,]
                {
                    { 1, "12 Lê Lợi, Q.1", "Hồ Chí Minh", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), new DateTime(1990, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Q.1", "binh.nguyen@gmail.com", "Nam", true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Nguyễn Văn Bình", "0987654321", 500000m, "Bến Nghé" },
                    { 2, "34 Nguyễn Trãi, Q.5", "Hồ Chí Minh", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), new DateTime(1985, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Q.5", "hoa.tran@gmail.com", "Nữ", true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Trần Thị Hoa", "0978123456", 350000m, "Phạm Ngũ Lão" }
                });

            migrationBuilder.InsertData(
                table: "MedicineGroups",
                columns: new[] { "Id", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Nhóm thuốc kháng sinh", true, "Kháng sinh" },
                    { 2, "Nhóm thuốc giảm đau", true, "Giảm đau" },
                    { 3, "Nhóm thuốc hạ sốt", true, "Hạ sốt" },
                    { 4, "Nhóm vitamin", true, "Vitamin" },
                    { 5, "Nhóm thuốc tiêu hóa", true, "Tiêu hóa" }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "CreatedDate", "CustomerId", "EmployeeId", "IsRead", "Message", "NotificationType", "RecipientType", "Title" },
                values: new object[] { 1, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), null, 1, false, "Paracetamol 500mg sắp hết hàng.", "Stock", "Employee", "Hết hàng Paracetamol" });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "CreatedDate", "Description", "DiscountValue", "EndDate", "IsActive", "IsPercentage", "LastModifiedDate", "MinOrderAmount", "Name", "PromotionType", "StartDate", "TargetCustomer" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Giảm 10% cho đơn từ 200k", 10m, new DateTime(2026, 1, 14, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), true, true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 200000, "Giảm giá mùa hè", "Discount", new DateTime(2025, 12, 15, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "All" },
                    { 2, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Tặng 1 hộp Vitamin C cho đơn trên 500k", 0m, new DateTime(2026, 1, 19, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), true, false, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 500000, "Tặng Vitamin C", "Gift", new DateTime(2025, 12, 20, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "VIP" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Address", "City", "ContactPerson", "CreatedDate", "Debt", "Email", "IsActive", "LastModifiedDate", "Name", "PhoneNumber", "TaxNumber" },
                values: new object[,]
                {
                    { 1, "288 Bis Nguyễn Văn Cừ, Cần Thơ", "Cần Thơ", "Nguyễn Văn A", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 0m, "contact@dhgpharma.com.vn", true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Công ty Dược Hậu Giang", "0909123456", "1800106840" },
                    { 2, "75 Yên Ninh, Hà Nội", "Hà Nội", "Trần Thị B", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 0m, "info@traphaco.com.vn", true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Công ty Traphaco", "0912345678", "0100108656" }
                });

            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "CreatedDate", "Email", "IsActive", "LastLoginDate", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { "d2af3ead-cbe2-42c8-b4b9-636f8340e9be", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "admin@example.com", true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "AQAAAAEAACcQAAAAEJ6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw==", "Admin", "Admin123" },
                    { "f15eee25-c953-4dd3-9e46-95ac317bccb5", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "nhanvien@example.com", true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "AQAAAAEAACcQAAAAEJ6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw==", "Employee", "NhanVien123" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedDate", "Department", "Email", "FullName", "LastModifiedDate", "PhoneNumber", "Position", "Salary", "StartDate", "Status", "UserAccountId" },
                values: new object[] { 1, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Bán hàng", "nhanvien@example.com", "Nhân Viên Mặc Định", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "0123456789", "Nhân Viên", 10000000m, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Active", "f15eee25-c953-4dd3-9e46-95ac317bccb5" });

            migrationBuilder.InsertData(
                table: "Medicines",
                columns: new[] { "Id", "Barcode", "CreatedDate", "CurrentStock", "Description", "Dosage", "ExpiryDate", "IsActive", "LastModifiedDate", "Manufacturer", "MedicineGroupId", "MinStockLevel", "Name", "Price", "Unit", "Usage" },
                values: new object[,]
                {
                    { 1, "8935206000012", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 200, "Thuốc hạ sốt, giảm đau", "1 viên/lần, 2-3 lần/ngày", new DateTime(2027, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "DHG Pharma", 3, 20, "Paracetamol 500mg", 1500m, "Viên", "Uống sau ăn" },
                    { 2, "8935206000029", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 150, "Kháng sinh phổ rộng", "1 viên/lần, 2 lần/ngày", new DateTime(2027, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Stada", 1, 15, "Amoxicillin 500mg", 2500m, "Viên", "Uống trước ăn" },
                    { 3, "8935206000036", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 300, "Tăng sức đề kháng", "1 viên/ngày", new DateTime(2027, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Traphaco", 4, 30, "Vitamin C 500mg", 1200m, "Viên", "Uống sau ăn" },
                    { 4, "8935206000043", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 100, "Điều trị tiêu chảy", "1 gói/lần, 2 lần/ngày", new DateTime(2027, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Ipsen", 5, 10, "Smecta", 8000m, "Gói", "Pha với nước" },
                    { 5, "8935206000050", new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 120, "Giảm đau, hạ sốt", "1 viên/lần, 2 lần/ngày", new DateTime(2027, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), true, new DateTime(2025, 12, 25, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Sanofi", 2, 12, "Ibuprofen 400mg", 2000m, "Viên", "Uống sau ăn" }
                });

            migrationBuilder.InsertData(
                table: "PurchaseOrders",
                columns: new[] { "Id", "CreatedDate", "DeliveryDate", "Notes", "OrderDate", "PurchaseOrderCode", "Status", "SupplierId", "TotalCost" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 15, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), new DateTime(2025, 12, 17, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "", new DateTime(2025, 12, 15, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "PO0001", "Received", 1, 1000000m },
                    { 2, new DateTime(2025, 12, 18, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), new DateTime(2025, 12, 20, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "", new DateTime(2025, 12, 18, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "PO0002", "Received", 2, 500000m }
                });

            migrationBuilder.InsertData(
                table: "InventoryHistories",
                columns: new[] { "Id", "CreatedDate", "MedicineId", "Notes", "Quantity", "Reason", "StockAfter", "StockBefore", "TransactionType" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 15, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 1, "", 100, "Nhập hàng", 200, 100, "Import" },
                    { 2, new DateTime(2025, 12, 15, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 2, "", 50, "Nhập hàng", 150, 100, "Import" }
                });

            migrationBuilder.InsertData(
                table: "MedicinePromotions",
                columns: new[] { "Id", "MedicineId", "PromotionId" },
                values: new object[] { 1, 3, 2 });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedDate", "CustomerId", "Discount", "EmployeeId", "Notes", "OrderCode", "OrderDate", "OrderStatus", "PaymentMethod", "SubTotal", "Tax", "Total" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 23, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 1, 10000m, 1, "", "ORD0001", new DateTime(2025, 12, 23, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Completed", "Cash", 100000m, 1000m, 91000m },
                    { 2, new DateTime(2025, 12, 24, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 2, 20000m, 1, "", "ORD0002", new DateTime(2025, 12, 24, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), "Completed", "Card", 200000m, 2000m, 182000m }
                });

            migrationBuilder.InsertData(
                table: "PurchaseOrderDetails",
                columns: new[] { "Id", "MedicineId", "PurchaseOrderId", "Quantity", "TotalCost", "UnitCost" },
                values: new object[,]
                {
                    { 1, 1, 1, 100, 120000m, 1200m },
                    { 2, 2, 1, 50, 100000m, 2000m },
                    { 3, 3, 2, 200, 200000m, 1000m }
                });

            migrationBuilder.InsertData(
                table: "WorkHistories",
                columns: new[] { "Id", "CreatedDate", "EmployeeId", "Notes", "OrdersProcessed", "Revenue", "Shift", "WorkDate" },
                values: new object[] { 1, new DateTime(2025, 12, 24, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890), 1, "", 5, 500000m, "Morning", new DateTime(2025, 12, 24, 22, 32, 37, 257, DateTimeKind.Local).AddTicks(4890) });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "Id", "MedicineId", "OrderId", "Quantity", "TotalPrice", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 10, 15000m, 1500m },
                    { 2, 3, 1, 5, 6000m, 1200m },
                    { 3, 2, 2, 20, 50000m, 2500m },
                    { 4, 4, 2, 10, 80000m, 8000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserAccountId",
                table: "Employees",
                column: "UserAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryHistories_MedicineId",
                table: "InventoryHistories",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePromotions_MedicineId",
                table: "MedicinePromotions",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePromotions_PromotionId",
                table: "MedicinePromotions",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_MedicineGroupId",
                table: "Medicines",
                column: "MedicineGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_MedicineId",
                table: "OrderDetails",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeId",
                table: "Orders",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_MedicineId",
                table: "PurchaseOrderDetails",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_PurchaseOrderId",
                table: "PurchaseOrderDetails",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHistories_EmployeeId",
                table: "WorkHistories",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryHistories");

            migrationBuilder.DropTable(
                name: "MedicinePromotions");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "WorkHistories");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "MedicineGroups");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
