using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PharmacyManagement.Data;
using PharmacyManagement.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System;
using System.IO;

namespace PharmacyManagement.Backend
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // 1. CẤU HÌNH DỊCH VỤ (SERVICES)
        public void ConfigureServices(IServiceCollection services)
        {
            // --- A. KẾT NỐI DATABASE ---
            services.AddDbContext<PharmacyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // --- B. ĐĂNG KÝ CÁC SERVICE (Bổ sung cho đủ) ---
            services.AddScoped<IMedicineService, MedicineService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IInventoryService, InventoryService>(); // Thêm cái này
            services.AddScoped<ISupplierService, SupplierService>();   // Thêm cái này

            // --- ORDER + PAYMENT ---
            services.AddScoped<IOrderService, OrderService>();
            services.AddSingleton<IVnPayService, VnPayService>();


            // --- C. CẤU HÌNH CORS (Cho phép React truy cập) ---
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000", "http://localhost:3001") // Cho phép cả port 3000 và 3001
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });

            // --- D. CẤU HÌNH JWT (ĐỂ ĐĂNG NHẬP ĐƯỢC) ---
            var jwtSettings = Configuration.GetSection("Jwt");
            var secretKey = jwtSettings["Key"];
            if (!string.IsNullOrEmpty(secretKey))
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings["Issuer"],
                            ValidAudience = jwtSettings["Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                        };
                    });
            }

            // Controllers & Swagger
            services.AddControllers();
            
            // Configure FormOptions for file uploads
            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue; // 2GB
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pharmacy Management API",
                    Version = "v1",
                    Description = "API for Pharmacy Management System"
                });
            });
        }

        // 2. CẤU HÌNH PIPELINE (MIDDLEWARE)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // CORS phải đặt lên đầu
            app.UseCors("AllowReactApp");

            // Auto-create PaymentTransactions table if it doesn't exist (for older databases without migrations)
            using (var scope = app.ApplicationServices.CreateScope())
            {
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<PharmacyContext>();
                    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[PaymentTransactions]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PaymentTransactions](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [OrderId] INT NOT NULL,
        [OrderCode] NVARCHAR(MAX) NULL,
        [Provider] NVARCHAR(MAX) NULL,
        [PaymentMethod] NVARCHAR(MAX) NULL,
        [Amount] DECIMAL(18,2) NOT NULL,
        [Currency] NVARCHAR(10) NULL,
        [Status] NVARCHAR(50) NULL,
        [TxnRef] NVARCHAR(MAX) NULL,
        [TransactionNo] NVARCHAR(MAX) NULL,
        [ResponseCode] NVARCHAR(MAX) NULL,
        [BankCode] NVARCHAR(MAX) NULL,
        [PayDate] NVARCHAR(MAX) NULL,
        [RawData] NVARCHAR(MAX) NULL,
        [CreatedAt] DATETIME2 NOT NULL,
        [UpdatedAt] DATETIME2 NULL
    );
END
");
                }
                catch
                {
                    // ignore (e.g., database not reachable at startup)
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pharmacy Management API v1"));
            }

            // --- QUAN TRỌNG: TẮT DÒNG NÀY ĐỂ TRÁNH LỖI NETWORK ERROR ---
            // app.UseHttpsRedirection(); 

            // --- CẤU HÌNH STATIC FILES (CHO UPLOAD AVATARS) ---
            app.UseStaticFiles();
            
            // Cấu hình để serve thư mục uploads
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
                RequestPath = "/uploads"
            });

            app.UseRouting();

            // --- QUAN TRỌNG: PHẢI CÓ Authentication TRƯỚC Authorization ---
            app.UseAuthentication(); // <--- Dòng này giúp Login hoạt động
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
