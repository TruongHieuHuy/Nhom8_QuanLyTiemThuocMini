# Tài Liệu Kỹ Thuật - Hệ Thống Quản Lý Tiệm Thuốc Mini

## 1. Kiến Trúc Hệ Thống

### Tổng Quan
Hệ thống sử dụng kiến trúc 3 tầng (Three-tier architecture):
- **Presentation Layer**: React Frontend
- **Business Logic Layer**: ASP.NET Core Services
- **Data Access Layer**: Entity Framework Core + SQL Server

### Sơ Đồ Kiến Trúc
```
┌─────────────────────────────────────────────────────────────┐
│                    Frontend (React)                         │
│  - Components, Pages, UI                                    │
└─────────────────────────────────────────────────────────────┘
                            ↕ HTTP/REST
┌─────────────────────────────────────────────────────────────┐
│              Backend API (ASP.NET Core)                      │
│  - Controllers → Services → Data Access                     │
│  - Authentication, Validation, Business Logic              │
└─────────────────────────────────────────────────────────────┘
                            ↕ SQL
┌─────────────────────────────────────────────────────────────┐
│                  Database (SQL Server)                       │
│  - Tables, Relationships, Constraints                       │
└─────────────────────────────────────────────────────────────┘
```

## 2. Database Schema

### Entity Relationship Diagram

```
┌─────────────────────┐         ┌────────────────────┐
│   MedicineGroup     │         │     Medicine       │
├─────────────────────┤         ├────────────────────┤
│ PK: Id              │◄────────│ PK: Id             │
│ Name                │ 1    *  │ Name               │
│ Description         │         │ MedicineGroupId (FK)│
│ IsActive            │         │ Price              │
└─────────────────────┘         │ CurrentStock       │
                                │ ExpiryDate         │
                                │ MinStockLevel      │
                                └────────────────────┘
                                      ↓ 1
                    ┌─────────────────────┬──────────────┐
                    │                     │              │
                  * │                   * │            * │
         ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
         │  OrderDetail     │  │InventoryHistory │  │MedicinePromotion │
         ├──────────────────┤  ├──────────────────┤  ├──────────────────┤
         │ PK: Id           │  │ PK: Id           │  │ PK: Id           │
         │ OrderId (FK)     │  │ MedicineId (FK)  │  │ MedicineId (FK)  │
         │ MedicineId (FK)  │  │ TransactionType  │  │ PromotionId (FK) │
         │ Quantity         │  │ Quantity         │  └──────────────────┘
         │ UnitPrice        │  │ StockBefore      │
         └──────────────────┘  │ StockAfter       │
                 ▲              │ CreatedDate      │
                 │              └──────────────────┘
               1 │
         ┌──────────────────┐
         │     Order        │
         ├──────────────────┤
         │ PK: Id           │
         │ OrderCode        │
         │ CustomerId (FK)  │
         │ EmployeeId (FK)  │
         │ OrderDate        │
         │ Total            │
         │ PaymentMethod    │
         └──────────────────┘
          ▲             ▲
        * │             │ *
          │             │
    ┌─────────────┐  ┌──────────────┐
    │  Customer   │  │  Employee    │
    ├─────────────┤  ├──────────────┤
    │ PK: Id      │  │ PK: Id       │
    │ Name        │  │ FullName     │
    │ Phone       │  │ Position     │
    │ Email       │  │ UserAccountId│
    │ Address     │  │ Salary       │
    │ TotalSpend  │  └──────────────┘
    └─────────────┘       ▼ 1
                  ┌──────────────────┐
                  │  UserAccount     │
                  ├──────────────────┤
                  │ PK: Id           │
                  │ Username         │
                  │ Email            │
                  │ Role             │
                  │ IsActive         │
                  └──────────────────┘

    ┌────────────────┐    ┌──────────────────────┐
    │   Supplier     │    │  PurchaseOrder       │
    ├────────────────┤    ├──────────────────────┤
    │ PK: Id         │◄───│ PK: Id               │
    │ Name           │ 1  │ SupplierId (FK)      │
    │ ContactPerson  │  * │ OrderDate            │
    │ Phone          │    │ TotalCost            │
    │ Email          │    │ Status               │
    │ Debt           │    └──────────────────────┘
    └────────────────┘           │ 1
                                 │ *
                        ┌────────────────┐
                        │PurchaseOrderDet│
                        ├────────────────┤
                        │ PK: Id         │
                        │ PurchaseOrderId│
                        │ MedicineId (FK)│
                        │ Quantity       │
                        │ UnitCost       │
                        └────────────────┘

    ┌──────────────┐      ┌────────────────┐
    │  Promotion   │      │ WorkHistory    │
    ├──────────────┤      ├────────────────┤
    │ PK: Id       │      │ PK: Id         │
    │ Name         │      │ EmployeeId (FK)│
    │ Discount     │      │ WorkDate       │
    │ IsPercentage │      │ Shift          │
    │ StartDate    │      │ OrdersProcessed│
    │ EndDate      │      │ Revenue        │
    └──────────────┘      └────────────────┘

    ┌──────────────────┐
    │  Notification    │
    ├──────────────────┤
    │ PK: Id           │
    │ Title            │
    │ Message          │
    │ RecipientType    │
    │ EmployeeId       │
    │ CustomerId       │
    │ NotificationType │
    │ IsRead           │
    │ CreatedDate      │
    └──────────────────┘
```

## 3. API Design

### REST Principles
- **GET**: Lấy dữ liệu
- **POST**: Tạo dữ liệu mới
- **PUT**: Cập nhật dữ liệu
- **DELETE**: Xóa dữ liệu

### Response Format
```json
// Success Response
{
  "data": {...},
  "message": "Success",
  "statusCode": 200
}

// Error Response
{
  "error": "Error message",
  "statusCode": 400,
  "details": {...}
}
```

## 4. Flow Quy Trình Kinh Doanh

### A. Quy Trình Tạo Đơn Hàng
```
1. Nhân viên chọn khách hàng → 2. Nhân viên chọn thuốc
   ↓
3. Nhập số lượng → 4. Hệ thống kiểm tra tồn kho
   ↓
5. Tính toán giá (giảm giá, thuế) → 6. Chọn phương thức thanh toán
   ↓
7. Lưu đơn hàng → 8. Cập nhật tồn kho
   ↓
9. Cập nhật tổng chi tiêu khách hàng → 10. Ghi lại lịch sử giao dịch
```

### B. Quy Trình Nhập Kho
```
1. Chọn nhà cung cấp → 2. Nhập danh sách thuốc
   ↓
3. Nhập số lượng, giá → 4. Tính tổng chi phí
   ↓
5. Lưu đơn nhập → 6. Cập nhật tồn kho
   ↓
7. Ghi lịch sử nhập kho → 8. Cập nhật công nợ nhà cung cấp
```

## 5. Security Implementation

### Authentication
- JWT Token-based authentication
- Token expiration: 60 minutes (configurable)
- Secure password storage using hashing

### Authorization
- Role-based access control (RBAC)
- Roles: Admin, Manager, Cashier, Pharmacist
- Endpoint-level protection

### Data Protection
- HTTPS/TLS encryption
- SQL injection prevention via Entity Framework
- XSS protection
- CORS configuration

## 6. Error Handling

### Error Codes
```
200: OK - Thành công
201: Created - Tạo thành công
204: No Content - Xóa thành công
400: Bad Request - Yêu cầu không hợp lệ
401: Unauthorized - Chưa xác thực
403: Forbidden - Không có quyền
404: Not Found - Không tìm thấy
500: Internal Server Error - Lỗi server
```

## 7. Performance Optimization

### Backend
- Caching sử dụng In-memory cache
- Async/await cho I/O operations
- Database indexing trên foreign keys
- Pagination cho large datasets

### Frontend
- Code splitting với React.lazy
- Image optimization
- CSS/JS minification
- Component memoization

## 8. Testing Strategy

### Unit Testing
- Backend: xUnit, Moq
- Frontend: Jest, React Testing Library

### Integration Testing
- API endpoint testing
- Database integration tests

### E2E Testing
- Cypress cho automation testing

## 9. Deployment

### Backend Deployment
```bash
# Build
dotnet publish -c Release

# Deploy to IIS
# hoặc Docker
docker build -t pharmacy-api .
docker run -d -p 5000:5000 pharmacy-api
```

### Frontend Deployment
```bash
# Build
npm run build

# Deploy to static hosting (Vercel, Netlify, AWS S3)
# hoặc Docker
docker build -t pharmacy-ui .
docker run -d -p 3000:3000 pharmacy-ui
```

## 10. Monitoring & Logging

### Logging Strategy
- Serilog cho structured logging
- Log levels: Debug, Information, Warning, Error, Fatal
- Logs lưu vào file, database, hoặc third-party services

### Metrics
- Response time monitoring
- Error rate tracking
- Database connection pooling
- API usage analytics

## 11. Backup & Recovery

- Daily database backups
- Transaction log backups
- Point-in-time recovery capability
- Disaster recovery plan

## 12. Scaling Considerations

- Horizontal scaling với load balancing
- Database replication
- Caching layer (Redis)
- API rate limiting

---

**Version**: 1.0.0  
**Last Updated**: December 2024
