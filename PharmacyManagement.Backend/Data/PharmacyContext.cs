using System;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Models;

namespace PharmacyManagement.Data
{
    public class PharmacyContext : DbContext
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> options) : base(options) { }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineGroup> MedicineGroups { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<InventoryHistory> InventoryHistories { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<WorkHistory> WorkHistories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<MedicinePromotion> MedicinePromotions { get; set; }
        
        // QUAN TRỌNG: Phải có 2 dòng này thì Dashboard mới đọc được đơn hàng
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. CẤU HÌNH QUAN HỆ ---
            modelBuilder.Entity<Medicine>()
                .HasOne(m => m.MedicineGroup)
                .WithMany(mg => mg.Medicines)
                .HasForeignKey(m => m.MedicineGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.UserAccount)
                .WithMany()
                .HasForeignKey(e => e.UserAccountId)
                .OnDelete(DeleteBehavior.SetNull);

            // Cấu hình cho Order (Quan trọng để không bị lỗi EmployeeId1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Employee)
                .WithMany() // Một nhân viên có nhiều đơn
                .HasForeignKey(o => o.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- CONFIGURE DECIMAL PRECISION FOR ALL PROPERTIES ---
            // Customer
            modelBuilder.Entity<Customer>()
                .Property(c => c.TotalSpending)
                .HasPrecision(18, 2);

            // Employee
            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);

            // Medicine
            modelBuilder.Entity<Medicine>()
                .Property(m => m.Price)
                .HasPrecision(18, 2);

            // Order
            modelBuilder.Entity<Order>()
                .Property(o => o.SubTotal)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Order>()
                .Property(o => o.Discount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Order>()
                .Property(o => o.Tax)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Order>()
                .Property(o => o.Total)
                .HasPrecision(18, 2);

            // OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.UnitPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.TotalPrice)
                .HasPrecision(18, 2);

            // PurchaseOrder
            modelBuilder.Entity<PurchaseOrder>()
                .Property(po => po.TotalCost)
                .HasPrecision(18, 2);

            // PurchaseOrderDetail
            modelBuilder.Entity<PurchaseOrderDetail>()
                .Property(pod => pod.UnitCost)
                .HasPrecision(18, 2);
            modelBuilder.Entity<PurchaseOrderDetail>()
                .Property(pod => pod.TotalCost)
                .HasPrecision(18, 2);

            // Promotion
            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountValue)
                .HasPrecision(18, 2);

            // Supplier
            modelBuilder.Entity<Supplier>()
                .Property(s => s.Debt)
                .HasPrecision(18, 2);

            // WorkHistory
            modelBuilder.Entity<WorkHistory>()
                .Property(wh => wh.Revenue)
                .HasPrecision(18, 2);

            // --- 2. NẠP DỮ LIỆU MẪU (SEED DATA) ---
            var now = DateTime.Now;

            // User & Employee
            modelBuilder.Entity<UserAccount>().HasData(
                new UserAccount { Id = 1, Username = "Admin123", Email = "admin@example.com", PasswordAccount = "123", Role = "Admin", IsActive = true, CreatedDate = now },
                new UserAccount { Id = 2, Username = "NhanVien123", Email = "nhanvien@example.com", PasswordAccount = "123", Role = "Employee", IsActive = true, CreatedDate = now }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FullName = "Nhân Viên Bán Hàng", Email = "nhanvien@example.com", PhoneNumber = "0123456789", Position = "Nhân Viên", Department = "Bán hàng", StartDate = now, Salary = 10000000, Status = "Active", UserAccountId = 2, CreatedDate = now, LastModifiedDate = now }
            );

            // Nhóm thuốc & Thuốc
            modelBuilder.Entity<MedicineGroup>().HasData(
                new MedicineGroup { Id = 1, Name = "Kháng sinh", Description = "Nhóm thuốc kháng sinh", IsActive = true },
                new MedicineGroup { Id = 2, Name = "Giảm đau", Description = "Nhóm thuốc giảm đau", IsActive = true },
                new MedicineGroup { Id = 3, Name = "Hạ sốt", Description = "Nhóm thuốc hạ sốt", IsActive = true },
                new MedicineGroup { Id = 4, Name = "Vitamin", Description = "Nhóm vitamin", IsActive = true },
                new MedicineGroup { Id = 5, Name = "Tiêu hóa", Description = "Nhóm thuốc tiêu hóa", IsActive = true }
            );

            modelBuilder.Entity<Medicine>().HasData(
                new Medicine { Id = 1, Name = "Paracetamol 500mg", Description = "Thuốc hạ sốt", MedicineGroupId = 3, Usage = "Uống sau ăn", Dosage = "1 viên/lần", Manufacturer = "DHG Pharma", Price = 1500, ExpiryDate = now.AddYears(2), CurrentStock = 200, MinStockLevel = 20, Unit = "Viên", Barcode = "8935206000012", CreatedDate = now, LastModifiedDate = now, IsActive = true },
                new Medicine { Id = 2, Name = "Amoxicillin 500mg", Description = "Kháng sinh", MedicineGroupId = 1, Usage = "Uống trước ăn", Dosage = "1 viên/lần", Manufacturer = "Stada", Price = 2500, ExpiryDate = now.AddYears(2), CurrentStock = 150, MinStockLevel = 15, Unit = "Viên", Barcode = "8935206000029", CreatedDate = now, LastModifiedDate = now, IsActive = true },
                new Medicine { Id = 3, Name = "Vitamin C 500mg", Description = "Tăng đề kháng", MedicineGroupId = 4, Usage = "Uống sau ăn", Dosage = "1 viên/ngày", Manufacturer = "Traphaco", Price = 1200, ExpiryDate = now.AddYears(2), CurrentStock = 300, MinStockLevel = 30, Unit = "Viên", Barcode = "8935206000036", CreatedDate = now, LastModifiedDate = now, IsActive = true },
                new Medicine { Id = 4, Name = "Smecta", Description = "Trị tiêu chảy", MedicineGroupId = 5, Usage = "Pha nước", Dosage = "1 gói/lần", Manufacturer = "Ipsen", Price = 8000, ExpiryDate = now.AddYears(2), CurrentStock = 100, MinStockLevel = 10, Unit = "Gói", Barcode = "8935206000043", CreatedDate = now, LastModifiedDate = now, IsActive = true },
                new Medicine { Id = 5, Name = "Ibuprofen 400mg", Description = "Giảm đau", MedicineGroupId = 2, Usage = "Uống sau ăn", Dosage = "1 viên/lần", Manufacturer = "Sanofi", Price = 2000, ExpiryDate = now.AddYears(2), CurrentStock = 120, MinStockLevel = 12, Unit = "Viên", Barcode = "8935206000050", CreatedDate = now, LastModifiedDate = now, IsActive = true }
            );

            // Khách hàng
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Nguyễn Văn Bình", PhoneNumber = "0987654321", Email = "binh@gmail.com", Address = "HCM", City = "HCM", District = "Q1", Ward = "Ben Nghe", DateOfBirth = new DateTime(1990, 1, 1), Gender = "Nam", TotalSpending = 500000, CreatedDate = now, LastModifiedDate = now, IsActive = true },
                new Customer { Id = 2, Name = "Trần Thị Hoa", PhoneNumber = "0978123456", Email = "hoa@gmail.com", Address = "HN", City = "HN", District = "HK", Ward = "Hang Bai", DateOfBirth = new DateTime(1995, 5, 5), Gender = "Nu", TotalSpending = 350000, CreatedDate = now, LastModifiedDate = now, IsActive = true }
            );

            // --- ĐÂY LÀ PHẦN QUAN TRỌNG: DỮ LIỆU ĐƠN HÀNG MẪU ---
            // Nếu thiếu phần này, Dashboard sẽ hiện số 0
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, OrderCode = "HD001", CustomerId = 1, EmployeeId = 1, OrderDate = now.AddDays(-1), SubTotal = 150000, Discount = 0, Tax = 0, Total = 150000, PaymentMethod = "Cash", OrderStatus = "Completed", Notes = "Khách quen", CreatedDate = now.AddDays(-1) },
                new Order { Id = 2, OrderCode = "HD002", CustomerId = 2, EmployeeId = 1, OrderDate = now, SubTotal = 200000, Discount = 20000, Tax = 0, Total = 180000, PaymentMethod = "Card", OrderStatus = "Completed", Notes = "", CreatedDate = now }
            );

            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail { Id = 1, OrderId = 1, MedicineId = 1, Quantity = 10, UnitPrice = 1500, TotalPrice = 15000 },
                new OrderDetail { Id = 2, OrderId = 2, MedicineId = 2, Quantity = 5, UnitPrice = 2500, TotalPrice = 12500 }
            );
        }
    }
}