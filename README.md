<<<<<<< HEAD
# Hệ Thống Quản Lý Tiệm Thuốc Mini

## Giới Thiệu
Hệ thống quản lý tiệm thuốc mini là một ứng dụng web toàn diện được xây dựng với:
- **Backend**: C# ASP.NET Core 6.0
- **Frontend**: React 18.2
- **Database**: SQL Server

## Các Tính Năng Chính

### 1. Quản Lý Thông Tin Thuốc
- ✅ Thêm, sửa, xóa thông tin thuốc
- ✅ Quản lý theo nhóm thuốc (kê đơn, không kê đơn, thực phẩm chức năng, v.v.)
- ✅ Hiển thị thông tin chi tiết (tên, công dụng, liều dùng, nhà sản xuất, giá, hạn sử dụng)
- ✅ Tìm kiếm thuốc theo tên, nhóm, giá tiền

### 2. Quản Lý Tồn Kho
- ✅ Theo dõi số lượng thuốc còn lại
- ✅ Cảnh báo khi tồn kho gần hết hoặc hết hàng
- ✅ Lịch sử nhập hàng và xuất kho chi tiết
- ✅ Quản lý lịch sử giao dịch kho

### 3. Quản Lý Bán Hàng
- ✅ Quản lý đơn hàng khách hàng
- ✅ Tính toán tổng giá trị, giảm giá, thuế
- ✅ Hỗ trợ các phương thức thanh toán (tiền mặt, thẻ ngân hàng, chuyển khoản)
- ✅ In hóa đơn

### 4. Quản Lý Khách Hàng
- ✅ Lưu trữ thông tin khách hàng (tên, địa chỉ, số điện thoại)
- ✅ Lịch sử giao dịch
- ✅ Cập nhật thông tin khách hàng

### 5. Quản Lý Nhân Viên
- ✅ Quản lý tài khoản nhân viên
- ✅ Cấp quyền truy cập (Admin, Manager, Cashier, Pharmacist)
- ✅ Lịch sử làm việc, ca làm việc

### 6. Báo Cáo & Thống Kê
- ✅ Thống kê doanh thu theo ngày, tuần, tháng
- ✅ Báo cáo tồn kho
- ✅ Báo cáo bán hàng (sản phẩm bán chạy, khách hàng mua nhiều)

### 7. Quản Lý Chương Trình Khuyến Mãi
- ✅ Tạo và quản lý các chương trình khuyến mãi
- ✅ Áp dụng theo khách hàng và thời gian

### 8. Quản Lý Nhà Cung Cấp
- ✅ Thêm, sửa, xóa thông tin nhà cung cấp
- ✅ Lịch sử nhập hàng
- ✅ Theo dõi công nợ

### 9. Quản Lý Thông Báo
- ✅ Thông báo cho nhân viên (nhập hàng, khuyến mãi)
- ✅ Thông báo cho khách hàng (sản phẩm mới, giảm giá)

## Cấu Trúc Dự Án

```
PharmacyManagement/
├── PharmacyManagement.Backend/
│   ├── Models/              # Entity models
│   ├── DTOs/                # Data Transfer Objects
│   ├── Controllers/         # API controllers
│   ├── Services/            # Business logic
│   ├── Data/                # DbContext
│   ├── Startup.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── PharmacyManagement.Backend.csproj
│
└── PharmacyManagement.Frontend/
    ├── public/
    │   └── index.html
    ├── src/
    │   ├── components/      # React components
    │   ├── pages/           # Page components
    │   ├── services/        # API services
    │   ├── styles/          # CSS files
    │   ├── App.js
    │   ├── index.js
    │   └── store.js         # Zustand store
    ├── package.json
    └── README.md
```

## Hướng Dẫn Cài Đặt

### Yêu Cầu Hệ Thống
- .NET 6.0 SDK
- Node.js 14+
- SQL Server 2019+

### Cài Đặt Backend

1. **Điều hướng đến thư mục backend:**
```bash
cd PharmacyManagement.Backend
```

2. **Khôi phục các gói NuGet:**
```bash
dotnet restore
```

3. **Cấu hình Database:**
   - Chỉnh sửa `appsettings.json` với connection string của SQL Server:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=PharmacyManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true"
   }
   ```

4. **Tạo và cập nhật Database:**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

5. **Chạy ứng dụng:**
```bash
dotnet run
```
   - API sẽ chạy tại: `http://localhost:5000`
   - Swagger Documentation: `http://localhost:5000/swagger`

### Cài Đặt Frontend

1. **Điều hướng đến thư mục frontend:**
```bash
cd PharmacyManagement.Frontend
```

2. **Cài đặt các gói npm:**
```bash
npm install
```

3. **Cấu hình biến môi trường:**
   - Tạo file `.env` với nội dung:
   ```
   REACT_APP_API_BASE_URL=http://localhost:5000/api
   ```

4. **Chạy ứng dụng:**
```bash
npm start
```
   - Frontend sẽ chạy tại: `http://localhost:3000`

## API Endpoints

### Medicines
- `GET /api/medicines` - Lấy danh sách tất cả thuốc
- `GET /api/medicines/{id}` - Lấy chi tiết thuốc
- `GET /api/medicines/search/{searchTerm}` - Tìm kiếm thuốc
- `GET /api/medicines/low-stock` - Danh sách thuốc sắp hết
- `GET /api/medicines/expired` - Danh sách thuốc hết hạn
- `POST /api/medicines` - Thêm thuốc mới
- `PUT /api/medicines` - Cập nhật thuốc
- `DELETE /api/medicines/{id}` - Xóa thuốc

### Customers
- `GET /api/customers` - Lấy danh sách khách hàng
- `GET /api/customers/{id}` - Chi tiết khách hàng
- `GET /api/customers/search/{searchTerm}` - Tìm kiếm khách hàng
- `POST /api/customers` - Thêm khách hàng
- `PUT /api/customers` - Cập nhật khách hàng
- `DELETE /api/customers/{id}` - Xóa khách hàng

### Orders
- `GET /api/orders` - Danh sách đơn hàng
- `GET /api/orders/{id}` - Chi tiết đơn hàng
- `GET /api/orders/customer/{customerId}` - Đơn hàng của khách hàng
- `POST /api/orders` - Tạo đơn hàng
- `PUT /api/orders/{id}/status` - Cập nhật trạng thái
- `DELETE /api/orders/{id}` - Xóa đơn hàng

### Reports
- `GET /api/reports/revenue/date/{date}` - Doanh thu theo ngày
- `GET /api/reports/top-medicines/{count}` - Sản phẩm bán chạy
- `GET /api/reports/top-customers/{count}` - Khách hàng hàng đầu
- `GET /api/reports/out-of-stock` - Thuốc hết hàng

## Thông Tin Đăng Nhập Demo
- **Username**: admin (hoặc bất kỳ giá trị nào)
- **Password**: password (hoặc bất kỳ giá trị nào)
- *Lưu ý: Đây là mock login, trong môi trường production cần xác thực thật với database*

## Công Nghệ Sử Dụng

### Backend
- ASP.NET Core 6.0
- Entity Framework Core 6.0
- SQL Server
- JWT Authentication
- Swagger/OpenAPI

### Frontend
- React 18.2
- Ant Design 5.0
- Axios
- React Router v6
- Zustand (State Management)
- Chart.js

## Các Tính Năng Nâng Cao

1. **Authentication & Authorization**
   - JWT Token-based authentication
   - Role-based access control (RBAC)

2. **Real-time Notifications**
   - Thông báo khi tồn kho gần hết
   - Thông báo khuyến mãi

3. **Reporting**
   - Báo cáo chi tiết doanh thu
   - Báo cáo tồn kho
   - Biểu đồ thống kê

4. **Search & Filter**
   - Tìm kiếm nhanh chóng
   - Lọc theo nhiều tiêu chí

## Hướng Phát Triển Tương Lai

1. **Mobile App**
   - React Native app cho nhân viên bán hàng

2. **Payment Integration**
   - Tích hợp thanh toán online (Stripe, PayPal)

3. **Advanced Analytics**
   - Machine Learning để dự báo tồn kho
   - Phân tích khách hàng

4. **Multi-location Support**
   - Quản lý nhiều cửa hàng

5. **Inventory Optimization**
   - Tự động đặt hàng

## Troubleshooting

### Database Connection Error
- Kiểm tra connection string trong `appsettings.json`
- Đảm bảo SQL Server đang chạy
- Kiểm tra quyền truy cập database

### CORS Error
- Đảm bảo backend được cấu hình CORS đúng
- Frontend URL phải được thêm vào `AllowedHosts`

### Port Already in Use
- Thay đổi port trong `appsettings.json`
- Hoặc kill process đang sử dụng port

## Hỗ Trợ & Liên Hệ
Để báo cáo lỗi hoặc yêu cầu tính năng, vui lòng tạo issue trong repository.

## Giấy Phép
Dự án này được cấp phép dưới MIT License.
=======
# Nhom8_QuanLyTiemThuocMini
>>>>>>> 3ecca45c580a28f8bb1b137d48cf405f028d0648
