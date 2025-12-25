using System;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Models;

namespace PharmacyManagement.Data
{
    public class PharmacyContext : DbContext
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> options)
            : base(options)
        {
        }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineGroup> MedicineGroups { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<InventoryHistory> InventoryHistories { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<MedicinePromotion> MedicinePromotions { get; set; }
        public DbSet<WorkHistory> WorkHistories { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Medicine relationships
            modelBuilder.Entity<Medicine>()
                .HasOne(m => m.MedicineGroup)
                .WithMany(mg => mg.Medicines)
                .HasForeignKey(m => m.MedicineGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Employee)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Medicine)
                .WithMany(m => m.OrderDetails)
                .HasForeignKey(od => od.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // PurchaseOrder relationships
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pod => pod.PurchaseOrder)
                .WithMany(po => po.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pod => pod.Medicine)
                .WithMany()
                .HasForeignKey(pod => pod.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // InventoryHistory
            modelBuilder.Entity<InventoryHistory>()
                .HasOne(ih => ih.Medicine)
                .WithMany(m => m.InventoryHistories)
                .HasForeignKey(ih => ih.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee relationships
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.UserAccount)
                .WithOne(ua => ua.Employee)
                .HasForeignKey<Employee>(e => e.UserAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkHistory>()
                .HasOne(wh => wh.Employee)
                .WithMany(e => e.WorkHistories)
                .HasForeignKey(wh => wh.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // MedicinePromotion
            modelBuilder.Entity<MedicinePromotion>()
                .HasOne(mp => mp.Medicine)
                .WithMany()
                .HasForeignKey(mp => mp.MedicineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MedicinePromotion>()
                .HasOne(mp => mp.Promotion)
                .WithMany(p => p.MedicinePromotions)
                .HasForeignKey(mp => mp.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed default admin and employee accounts
            var adminId = Guid.NewGuid().ToString();
            var employeeId = Guid.NewGuid().ToString();
            var now = DateTime.Now;

            modelBuilder.Entity<UserAccount>().HasData(
                new UserAccount
                {
                    Id = adminId,
                    Username = "Admin123",
                    Email = "admin@example.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEJ6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw==", // Admin@123 (hash placeholder)
                    Role = "Admin",
                    IsActive = true,
                    CreatedDate = now,
                    LastLoginDate = now
                },
                new UserAccount
                {
                    Id = employeeId,
                    Username = "NhanVien123",
                    Email = "nhanvien@example.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEJ6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw1n6Qw==", // NhanVien@123 (hash placeholder)
                    Role = "Employee",
                    IsActive = true,
                    CreatedDate = now,
                    LastLoginDate = now
                }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    FullName = "Nhân Viên Mặc Định",
                    Email = "nhanvien@example.com",
                    PhoneNumber = "0123456789",
                    Position = "Nhân Viên",
                    Department = "Bán hàng",
                    StartDate = now,
                    Salary = 10000000,
                    Status = "Active",
                    UserAccountId = employeeId,
                    CreatedDate = now,
                    LastModifiedDate = now
                }
            );

            // Seed MedicineGroups
            modelBuilder.Entity<MedicineGroup>().HasData(
                new MedicineGroup { Id = 1, Name = "Kháng sinh", Description = "Nhóm thuốc kháng sinh", IsActive = true },
                new MedicineGroup { Id = 2, Name = "Giảm đau", Description = "Nhóm thuốc giảm đau", IsActive = true },
                new MedicineGroup { Id = 3, Name = "Hạ sốt", Description = "Nhóm thuốc hạ sốt", IsActive = true },
                new MedicineGroup { Id = 4, Name = "Vitamin", Description = "Nhóm vitamin", IsActive = true },
                new MedicineGroup { Id = 5, Name = "Tiêu hóa", Description = "Nhóm thuốc tiêu hóa", IsActive = true }
            );

            // Seed Medicines
            modelBuilder.Entity<Medicine>().HasData(
                new Medicine { Id = 1, Name = "Paracetamol 500mg", Description = "Thuốc hạ sốt, giảm đau", MedicineGroupId = 3, Usage = "Uống sau ăn", Dosage = "1 viên/lần, 2-3 lần/ngày", Manufacturer = "DHG Pharma", Price = 1500, ExpiryDate = now.AddYears(2), CurrentStock = 200, MinStockLevel = 20, Unit = "Viên", Barcode = "8935206000012", CreatedDate = now, LastModifiedDate = now, IsActive = true },
                new Medicine { Id = 2, Name = "Amoxicillin 500mg", Description = "Kháng sinh phổ rộng", MedicineGroupId = 1, Usage = "Uống trước ăn", Dosage = "1 viên/lần, 2 lần/ngày", Manufacturer = "Stada", Price = 2500, ExpiryDate = now.AddYears(2), CurrentStock = 150, MinStockLevel = 15, Unit = "Viên", Barcode = "8935206000029", CreatedDate = now, LastModifiedDate = now, IsActive = true },
                new Medicine { Id = 3, Name = "Vitamin C 500mg", Description = "Tăng sức đề kháng", MedicineGroupId = 4, Usage = "Uống sau ăn", Dosage = "1 viên/ngày", Manufacturer = "Traphaco", Price = 1200, ExpiryDate = now.AddYears(2), CurrentStock = 300, MinStockLevel = 30, Unit = "Viên", Barcode = "8935206000036", CreatedDate = now, LastModifiedDate = now, IsActive = true },
                new Medicine { Id = 4, Name = "Smecta", Description = "Điều trị tiêu chảy", MedicineGroupId = 5, Usage = "Pha với nước", Dosage = "1 gói/lần, 2 lần/ngày", Manufacturer = "Ipsen", Price = 8000, ExpiryDate = now.AddYears(2), CurrentStock = 100, MinStockLevel = 10, Unit = "Gói", Barcode = "8935206000043", CreatedDate = now, LastModifiedDate = now, IsActive = true },
                new Medicine { Id = 5, Name = "Ibuprofen 400mg", Description = "Giảm đau, hạ sốt", MedicineGroupId = 2, Usage = "Uống sau ăn", Dosage = "1 viên/lần, 2 lần/ngày", Manufacturer = "Sanofi", Price = 2000, ExpiryDate = now.AddYears(2), CurrentStock = 120, MinStockLevel = 12, Unit = "Viên", Barcode = "8935206000050", CreatedDate = now, LastModifiedDate = now, IsActive = true }
            );

            // Seed Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, Name = "Công ty Dược Hậu Giang", ContactPerson = "Nguyễn Văn A", PhoneNumber = "0909123456", Email = "contact@dhgpharma.com.vn", Address = "288 Bis Nguyễn Văn Cừ, Cần Thơ", City = "Cần Thơ", TaxNumber = "1800106840", Debt = 0, IsActive = true, CreatedDate = now, LastModifiedDate = now },
                new Supplier { Id = 2, Name = "Công ty Traphaco", ContactPerson = "Trần Thị B", PhoneNumber = "0912345678", Email = "info@traphaco.com.vn", Address = "75 Yên Ninh, Hà Nội", City = "Hà Nội", TaxNumber = "0100108656", Debt = 0, IsActive = true, CreatedDate = now, LastModifiedDate = now }
            );

            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Nguyễn Văn Bình", PhoneNumber = "0987654321", Email = "binh.nguyen@gmail.com", Address = "12 Lê Lợi, Q.1", City = "Hồ Chí Minh", District = "Q.1", Ward = "Bến Nghé", DateOfBirth = new DateTime(1990, 5, 20), Gender = "Nam", TotalSpending = 500000, CreatedDate = now, LastModifiedDate = now, IsActive = true },
                new Customer { Id = 2, Name = "Trần Thị Hoa", PhoneNumber = "0978123456", Email = "hoa.tran@gmail.com", Address = "34 Nguyễn Trãi, Q.5", City = "Hồ Chí Minh", District = "Q.5", Ward = "Phạm Ngũ Lão", DateOfBirth = new DateTime(1985, 8, 15), Gender = "Nữ", TotalSpending = 350000, CreatedDate = now, LastModifiedDate = now, IsActive = true }
            );

            // Seed Promotions
            modelBuilder.Entity<Promotion>().HasData(
                new Promotion { Id = 1, Name = "Giảm giá mùa hè", Description = "Giảm 10% cho đơn từ 200k", PromotionType = "Discount", DiscountValue = 10, IsPercentage = true, StartDate = now.AddDays(-10), EndDate = now.AddDays(20), MinOrderAmount = 200000, TargetCustomer = "All", IsActive = true, CreatedDate = now, LastModifiedDate = now },
                new Promotion { Id = 2, Name = "Tặng Vitamin C", Description = "Tặng 1 hộp Vitamin C cho đơn trên 500k", PromotionType = "Gift", DiscountValue = 0, IsPercentage = false, StartDate = now.AddDays(-5), EndDate = now.AddDays(25), MinOrderAmount = 500000, TargetCustomer = "VIP", IsActive = true, CreatedDate = now, LastModifiedDate = now }
            );

            // Seed Orders
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, OrderCode = "ORD0001", CustomerId = 1, EmployeeId = 1, OrderDate = now.AddDays(-2), SubTotal = 100000, Discount = 10000, Tax = 1000, Total = 91000, PaymentMethod = "Cash", OrderStatus = "Completed", Notes = "", CreatedDate = now.AddDays(-2) },
                new Order { Id = 2, OrderCode = "ORD0002", CustomerId = 2, EmployeeId = 1, OrderDate = now.AddDays(-1), SubTotal = 200000, Discount = 20000, Tax = 2000, Total = 182000, PaymentMethod = "Card", OrderStatus = "Completed", Notes = "", CreatedDate = now.AddDays(-1) }
            );

            // Seed OrderDetails
            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail { Id = 1, OrderId = 1, MedicineId = 1, Quantity = 10, UnitPrice = 1500, TotalPrice = 15000 },
                new OrderDetail { Id = 2, OrderId = 1, MedicineId = 3, Quantity = 5, UnitPrice = 1200, TotalPrice = 6000 },
                new OrderDetail { Id = 3, OrderId = 2, MedicineId = 2, Quantity = 20, UnitPrice = 2500, TotalPrice = 50000 },
                new OrderDetail { Id = 4, OrderId = 2, MedicineId = 4, Quantity = 10, UnitPrice = 8000, TotalPrice = 80000 }
            );

            // Seed PurchaseOrders
            modelBuilder.Entity<PurchaseOrder>().HasData(
                new PurchaseOrder { Id = 1, PurchaseOrderCode = "PO0001", SupplierId = 1, OrderDate = now.AddDays(-10), DeliveryDate = now.AddDays(-8), TotalCost = 1000000, Status = "Received", Notes = "", CreatedDate = now.AddDays(-10) },
                new PurchaseOrder { Id = 2, PurchaseOrderCode = "PO0002", SupplierId = 2, OrderDate = now.AddDays(-7), DeliveryDate = now.AddDays(-5), TotalCost = 500000, Status = "Received", Notes = "", CreatedDate = now.AddDays(-7) }
            );

            // Seed PurchaseOrderDetails
            modelBuilder.Entity<PurchaseOrderDetail>().HasData(
                new PurchaseOrderDetail { Id = 1, PurchaseOrderId = 1, MedicineId = 1, Quantity = 100, UnitCost = 1200, TotalCost = 120000 },
                new PurchaseOrderDetail { Id = 2, PurchaseOrderId = 1, MedicineId = 2, Quantity = 50, UnitCost = 2000, TotalCost = 100000 },
                new PurchaseOrderDetail { Id = 3, PurchaseOrderId = 2, MedicineId = 3, Quantity = 200, UnitCost = 1000, TotalCost = 200000 }
            );

            // Seed InventoryHistory
            modelBuilder.Entity<InventoryHistory>().HasData(
                new InventoryHistory { Id = 1, MedicineId = 1, TransactionType = "Import", Quantity = 100, StockBefore = 100, StockAfter = 200, Reason = "Nhập hàng", Notes = "", CreatedDate = now.AddDays(-10) },
                new InventoryHistory { Id = 2, MedicineId = 2, TransactionType = "Import", Quantity = 50, StockBefore = 100, StockAfter = 150, Reason = "Nhập hàng", Notes = "", CreatedDate = now.AddDays(-10) }
            );

            // Seed MedicinePromotion
            modelBuilder.Entity<MedicinePromotion>().HasData(
                new MedicinePromotion { Id = 1, MedicineId = 3, PromotionId = 2 }
            );

            // Seed Notification
            modelBuilder.Entity<Notification>().HasData(
                new Notification { Id = 1, Title = "Hết hàng Paracetamol", Message = "Paracetamol 500mg sắp hết hàng.", RecipientType = "Employee", EmployeeId = 1, CustomerId = null, NotificationType = "Stock", IsRead = false, CreatedDate = now }
            );

            // Seed WorkHistory
            modelBuilder.Entity<WorkHistory>().HasData(
                new WorkHistory { Id = 1, EmployeeId = 1, WorkDate = now.AddDays(-1), Shift = "Morning", OrdersProcessed = 5, Revenue = 500000, Notes = "", CreatedDate = now.AddDays(-1) }
            );
        }
    }
}
