# Project Structure Overview

## 📁 Cấu Trúc Thư Mục

```
QuanLyTiemThuocMini_Nhom8/
│
├── README.md                           # Tổng quan dự án
├── QUICKSTART.md                       # Hướng dẫn khởi động nhanh
├── USER_GUIDE.md                       # Hướng dẫn sử dụng
├── TECHNICAL_DOCUMENTATION.md          # Tài liệu kỹ thuật
│
├── PharmacyManagement.Backend/        # Backend ASP.NET Core
│   ├── Models/                        # Entity Models
│   │   ├── Medicine.cs
│   │   ├── MedicineGroup.cs
│   │   ├── Customer.cs
│   │   ├── Employee.cs
│   │   ├── UserAccount.cs
│   │   ├── Order.cs
│   │   ├── OrderDetail.cs
│   │   ├── Supplier.cs
│   │   ├── PurchaseOrder.cs
│   │   ├── PurchaseOrderDetail.cs
│   │   ├── InventoryHistory.cs
│   │   ├── Promotion.cs
│   │   ├── MedicinePromotion.cs
│   │   ├── WorkHistory.cs
│   │   └── Notification.cs
│   │
│   ├── DTOs/                          # Data Transfer Objects
│   │   ├── MedicineDTO.cs
│   │   ├── MedicineGroupDTO.cs
│   │   ├── CustomerDTO.cs
│   │   ├── OrderDTO.cs
│   │   ├── EmployeeDTO.cs
│   │   ├── SupplierDTO.cs
│   │   ├── PromotionDTO.cs
│   │   └── InventoryHistoryDTO.cs
│   │
│   ├── Controllers/                   # API Controllers
│   │   ├── AuthController.cs
│   │   ├── MedicinesController.cs
│   │   ├── CustomersController.cs
│   │   ├── OrdersController.cs
│   │   ├── ReportsController.cs
│   │   ├── SuppliersController.cs
│   │   └── InventoryController.cs
│   │
│   ├── Services/                      # Business Logic
│   │   ├── IMedicineService.cs
│   │   ├── ICustomerService.cs
│   │   ├── IOrderService.cs
│   │   ├── IReportService.cs
│   │   ├── ISupplierService.cs
│   │   └── IInventoryService.cs
│   │
│   ├── Data/
│   │   ├── PharmacyContext.cs         # DbContext
│   │   └── Migrations/                # EF Migrations
│   │
│   ├── Startup.cs                     # Startup configuration
│   ├── Program.cs                     # Entry point
│   ├── appsettings.json              # Configuration
│   ├── appsettings.Development.json  # Dev configuration
│   └── PharmacyManagement.Backend.csproj
│
└── PharmacyManagement.Frontend/       # React Frontend
    ├── public/
    │   └── index.html                 # HTML entry point
    │
    ├── src/
    │   ├── components/                # Reusable Components
    │   │   ├── Layout.js              # Main layout
    │   │   ├── Layout.css
    │   │   ├── MedicineList.js
    │   │   └── CustomerList.js
    │   │
    │   ├── pages/                     # Page Components
    │   │   ├── Dashboard.js
    │   │   ├── MedicinesPage.js
    │   │   ├── CustomersPage.js
    │   │   ├── OrdersPage.js
    │   │   ├── ReportsPage.js
    │   │   ├── InventoryPage.js
    │   │   ├── LoginPage.js
    │   │   └── LoginPage.css
    │   │
    │   ├── services/                  # API Services
    │   │   ├── apiClient.js           # HTTP Client
    │   │   └── index.js               # Service exports
    │   │
    │   ├── styles/                    # Global Styles
    │   │
    │   ├── utils/                     # Utilities
    │   │
    │   ├── App.js                     # Main app component
    │   ├── App.css
    │   ├── index.js                   # Entry point
    │   └── store.js                   # Zustand store
    │
    ├── .env.example                   # Environment template
    ├── package.json                   # Dependencies
    └── README.md
```

## 🔑 File Quan Trọng

### Backend
| File | Mục Đích |
|------|---------|
| `Startup.cs` | Cấu hình services và middleware |
| `Program.cs` | Entry point ứng dụng |
| `PharmacyContext.cs` | Database context, relationships |
| `Startup.cs` | DI configuration, CORS setup |
| `appsettings.json` | Connection string, JWT settings |

### Frontend
| File | Mục Đích |
|------|---------|
| `App.js` | Router setup, main routes |
| `index.js` | React entry point |
| `store.js` | Zustand state management |
| `apiClient.js` | HTTP client wrapper |
| `Layout.js` | Navigation layout |

## 📦 Các Module Chính

### 1. Authentication & Authorization
- `AuthController.cs` - Login endpoint
- JWT token generation
- Role-based access control

### 2. Medicine Management
- `MedicineService.cs` - CRUD operations
- Search, filter, low stock alerts
- Expiry date tracking

### 3. Customer Management
- `CustomerService.cs` - Customer CRUD
- Transaction history
- Spending tracking

### 4. Order Management
- `OrderService.cs` - Order creation, processing
- Auto inventory update
- Payment method support

### 5. Reporting
- `ReportService.cs` - Revenue reports
- Top products, customers
- Out of stock reporting

### 6. Inventory Tracking
- `InventoryService.cs` - Stock history
- Transaction logging
- Stock alerts

## 🛠️ Build & Deployment

### Backend Build
```bash
cd PharmacyManagement.Backend
dotnet publish -c Release -o ./publish
```

### Frontend Build
```bash
cd PharmacyManagement.Frontend
npm run build
```

## 📋 Dependencies

### Backend
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- System.IdentityModel.Tokens.Jwt
- Microsoft.AspNetCore.Authentication.JwtBearer
- Swashbuckle.AspNetCore

### Frontend
- react & react-dom
- react-router-dom
- axios
- antd (Ant Design)
- chart.js & react-chartjs-2
- zustand
- date-fns

## 🔄 Data Flow

### Creating Order
```
Frontend (OrderPage)
  ↓ (HTTP POST)
Backend (OrdersController)
  ↓
OrderService.CreateOrderAsync()
  ↓
1. Create Order
2. Create OrderDetails
3. Update Medicine Stock
4. Add InventoryHistory
5. Update Customer Spending
  ↓
Save to Database
  ↓ (HTTP Response)
Frontend (Success Message)
```

## 🔐 Security Layers

1. **Frontend**: Authentication guard in routes
2. **API**: JWT validation in middleware
3. **Database**: FK constraints, data validation
4. **Input**: Model validation in DTOs

## 📊 Database Relations

```
Customer (1) ──── (*) Order
         │
         └──────── (*) WorkHistory

Medicine (1) ──── (*) OrderDetail
         │
         └──────── (*) InventoryHistory
         │
         └──────── (*) MedicinePromotion

Supplier (1) ──── (*) PurchaseOrder
         │
         └──────── (*) PurchaseOrderDetail

Employee (1) ──── (*) Order
         │
         └──────── (*) UserAccount

Promotion (1) ──── (*) MedicinePromotion

MedicineGroup (1) ──── (*) Medicine
```

## 🚀 Starting Points

### For Developers
1. Start with `README.md`
2. Review `TECHNICAL_DOCUMENTATION.md`
3. Check `Startup.cs` for DI setup
4. Explore `Controllers/` for API design
5. Study `Services/` for business logic

### For Users
1. Read `USER_GUIDE.md`
2. Follow `QUICKSTART.md`
3. Try demo features
4. Read tooltips and help text in UI

## 📝 Code Conventions

### Naming
- Controllers: `{Entity}Controller`
- Services: `I{Entity}Service` (interface)
- Models: PascalCase
- Properties: PascalCase
- Private fields: _camelCase

### Folder Structure
- Keep models in `Models/`
- DTOs in `DTOs/`
- Services in `Services/`
- Controllers in `Controllers/`

## 🐛 Common Issues & Solutions

| Issue | Solution |
|-------|----------|
| Database connection error | Check connection string in appsettings.json |
| CORS error | Verify CORS settings in Startup.cs |
| JWT expired | Token expires in 60 minutes by default |
| Port conflict | Change port in launchSettings.json |
| Frontend won't connect | Ensure backend is running on port 5000 |

---

**Last Updated**: December 2024
**Version**: 1.0.0
