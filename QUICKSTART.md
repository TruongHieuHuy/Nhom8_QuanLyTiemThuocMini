# Pharmacy Management System - Quick Start Guide

## ⚡ Khởi Động Nhanh

### Backend
```bash
cd PharmacyManagement.Backend
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend
```bash
cd PharmacyManagement.Frontend
npm install
npm start
```

## 📋 Danh Sách File Quan Trọng

### Backend
- `Startup.cs` - Cấu hình ứng dụng
- `Program.cs` - Entry point
- `appsettings.json` - Cấu hình
- `Models/` - Entity models
- `Controllers/` - API endpoints
- `Services/` - Business logic
- `Data/PharmacyContext.cs` - Database context

### Frontend
- `src/App.js` - Main component
- `src/pages/` - Page components
- `src/components/` - Reusable components
- `src/services/` - API services
- `src/store.js` - State management
- `package.json` - Dependencies

## 🔧 Cấu Hình Quan Trọng

### Database Connection
Sửa `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=PharmacyManagementDb;..."
}
```

### API Base URL
Tạo `.env` trong Frontend:
```
REACT_APP_API_BASE_URL=http://localhost:5000/api
```

## 🚀 Tính Năng Chính

✅ Quản lý thuốc  
✅ Quản lý khách hàng  
✅ Quản lý đơn hàng  
✅ Báo cáo doanh thu  
✅ Quản lý tồn kho  
✅ Xác thực người dùng  

## 📚 Tài Liệu

- `README.md` - Tổng quan dự án
- `USER_GUIDE.md` - Hướng dẫn sử dụng
- `TECHNICAL_DOCUMENTATION.md` - Tài liệu kỹ thuật

## 🐛 Troubleshooting

### Port Đã Được Sử Dụng
```bash
# Find process on port 5000
netstat -ano | findstr :5000
# Kill process
taskkill /PID <PID> /F
```

### Database Connection Error
- Kiểm tra SQL Server đang chạy
- Kiểm tra connection string
- Chạy: `dotnet ef database update`

### CORS Error
- Kiểm tra backend cấu hình CORS
- Kiểm tra frontend URL

---

Mở tại: `http://localhost:3000`
