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
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
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

            // PaymentTransaction
            modelBuilder.Entity<PaymentTransaction>()
                .Property(pt => pt.Amount)
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

            // Nhà cung cấp
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, Name = "Công ty Dược phẩm DHG", Email = "dhg@dhgpharma.com", PhoneNumber = "0283823456", Address = "Quận Bình Thạnh, TP.HCM", City = "TP.HCM", Debt = 0, IsActive = true, CreatedDate = now, LastModifiedDate = now },
                new Supplier { Id = 2, Name = "Công ty TNHH Stada Việt Nam", Email = "info@stada.vn", PhoneNumber = "0283834567", Address = "Quận 10, TP.HCM", City = "TP.HCM", Debt = 5000000, IsActive = true, CreatedDate = now, LastModifiedDate = now },
                new Supplier { Id = 3, Name = "Traphaco", Email = "contact@traphaco.com.vn", PhoneNumber = "0903123456", Address = "Thanh Xuân, Hà Nội", City = "Hà Nội", Debt = 0, IsActive = true, CreatedDate = now, LastModifiedDate = now }
            );

            // Khuyến mãi
            modelBuilder.Entity<Promotion>().HasData(
                new Promotion { Id = 1, Name = "Giảm giá mùa hè", Description = "Giảm 10% cho đơn hàng trên 500k", PromotionType = "Discount", DiscountValue = 10, IsPercentage = true, StartDate = now.AddDays(-30), EndDate = now.AddDays(30), MinOrderAmount = 500000, TargetCustomer = "All", IsActive = true, CreatedDate = now.AddDays(-30), LastModifiedDate = now },
                new Promotion { Id = 2, Name = "Khuyến mãi thuốc vitamin", Description = "Giảm 50k cho vitamin", PromotionType = "Discount", DiscountValue = 50000, IsPercentage = false, StartDate = now.AddDays(-15), EndDate = now.AddDays(15), MinOrderAmount = 0, TargetCustomer = "All", IsActive = true, CreatedDate = now.AddDays(-15), LastModifiedDate = now }
            );

            // Liên kết thuốc - khuyến mãi
            modelBuilder.Entity<MedicinePromotion>().HasData(
                new MedicinePromotion { Id = 1, MedicineId = 3, PromotionId = 2 }, // Vitamin C có KM
                new MedicinePromotion { Id = 2, MedicineId = 4, PromotionId = 1 }  // Smecta có KM mùa hè
            );

            // Đơn nhập hàng
            modelBuilder.Entity<PurchaseOrder>().HasData(
                new PurchaseOrder { Id = 1, PurchaseOrderCode = "PO001", SupplierId = 1, OrderDate = now.AddDays(-10), DeliveryDate = now.AddDays(-8), TotalCost = 15000000, Status = "Received", Notes = "Đơn nhập hàng đầu tháng", CreatedDate = now.AddDays(-10) },
                new PurchaseOrder { Id = 2, PurchaseOrderCode = "PO002", SupplierId = 2, OrderDate = now.AddDays(-5), DeliveryDate = now.AddDays(-3), TotalCost = 8500000, Status = "Received", Notes = "", CreatedDate = now.AddDays(-5) },
                new PurchaseOrder { Id = 3, PurchaseOrderCode = "PO003", SupplierId = 3, OrderDate = now.AddDays(-1), DeliveryDate = now.AddDays(2), TotalCost = 5000000, Status = "Pending", Notes = "Đơn hàng đang chờ giao", CreatedDate = now.AddDays(-1) }
            );

            // Chi tiết đơn nhập hàng
            modelBuilder.Entity<PurchaseOrderDetail>().HasData(
                new PurchaseOrderDetail { Id = 1, PurchaseOrderId = 1, MedicineId = 1, Quantity = 500, UnitCost = 1000, TotalCost = 500000 },
                new PurchaseOrderDetail { Id = 2, PurchaseOrderId = 1, MedicineId = 2, Quantity = 300, UnitCost = 2000, TotalCost = 600000 },
                new PurchaseOrderDetail { Id = 3, PurchaseOrderId = 2, MedicineId = 3, Quantity = 1000, UnitCost = 800, TotalCost = 800000 },
                new PurchaseOrderDetail { Id = 4, PurchaseOrderId = 2, MedicineId = 4, Quantity = 200, UnitCost = 6000, TotalCost = 1200000 },
                new PurchaseOrderDetail { Id = 5, PurchaseOrderId = 3, MedicineId = 5, Quantity = 400, UnitCost = 1500, TotalCost = 600000 }
            );

            // Thông báo
            modelBuilder.Entity<Notification>().HasData(
                new Notification { Id = 1, Title = "Thuốc sắp hết hạn", Message = "Có 5 loại thuốc sắp hết hạn trong tháng này", RecipientType = "Employee", EmployeeId = 1, NotificationType = "Stock", IsRead = false, CreatedDate = now.AddDays(-2) },
                new Notification { Id = 2, Title = "Đơn hàng mới", Message = "Có 3 đơn hàng mới cần xử lý", RecipientType = "Employee", EmployeeId = 1, NotificationType = "System", IsRead = true, CreatedDate = now.AddDays(-1) },
                new Notification { Id = 3, Title = "Tồn kho thấp", Message = "Paracetamol và Amoxicillin sắp hết", RecipientType = "Employee", EmployeeId = 1, NotificationType = "Stock", IsRead = false, CreatedDate = now }
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

            // Giao dịch thanh toán (VNPay, MoMo, etc.)
            modelBuilder.Entity<PaymentTransaction>().HasData(
                new PaymentTransaction { Id = 1, OrderId = 2, OrderCode = "HD002", Provider = "VNPay", PaymentMethod = "Bank", Amount = 180000, Currency = "VND", Status = "Success", TxnRef = "HD002", TransactionNo = "VNP" + now.Ticks.ToString().Substring(0, 10), ResponseCode = "00", BankCode = "NCB", PayDate = now.ToString("yyyyMMddHHmmss"), CreatedAt = now, RawData = "{\"vnp_Amount\":\"18000000\",\"vnp_ResponseCode\":\"00\"}" }
            );
        }
    }
}