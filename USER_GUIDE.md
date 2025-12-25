# Hướng Dẫn Sử Dụng Hệ Thống Quản Lý Tiệm Thuốc Mini

## Mục Lục
1. [Giới Thiệu](#giới-thiệu)
2. [Đăng Nhập Hệ Thống](#đăng-nhập-hệ-thống)
3. [Dashboard](#dashboard)
4. [Quản Lý Thuốc](#quản-lý-thuốc)
5. [Quản Lý Khách Hàng](#quản-lý-khách-hàng)
6. [Quản Lý Đơn Hàng](#quản-lý-đơn-hàng)
7. [Báo Cáo Thống Kê](#báo-cáo-thống-kê)
8. [Quản Lý Tồn Kho](#quản-lý-tồn-kho)
9. [Xử Lý Sự Cố](#xử-lý-sự-cố)

## Giới Thiệu

Hệ thống Quản Lý Tiệm Thuốc Mini được thiết kế để giúp quản lý hiệu quả các hoạt động bán hàng, tồn kho, và dữ liệu khách hàng tại tiệm thuốc.

### Các Tính Năng Chính:
- 📊 Bảng điều khiển với thống kê real-time
- 💊 Quản lý thuốc chi tiết
- 👥 Quản lý thông tin khách hàng
- 🛒 Xử lý đơn hàng nhanh chóng
- 📈 Báo cáo và thống kê toàn diện
- 📦 Theo dõi tồn kho tự động

## Đăng Nhập Hệ Thống

### Bước 1: Truy Cập Ứng Dụng
- Mở trình duyệt web
- Nhập địa chỉ: `http://localhost:3000`

### Bước 2: Nhập Thông Tin Đăng Nhập
- **Tên đăng nhập**: admin (hoặc username bất kỳ)
- **Mật khẩu**: password (hoặc password bất kỳ)
- Chọn "Nhớ tôi" nếu muốn lưu tài khoản (tùy chọn)

### Bước 3: Đăng Nhập
- Nhấp nút "Đăng nhập"
- Chờ tải trang và bảng điều khiển sẽ hiển thị

## Dashboard

### Bảng Điều Khiển Chính
Hiển thị các thông tin quan trọng:

**Các Chỉ Số Chính:**
- **Tổng Đơn Hàng**: Số lượng đơn hàng trong hệ thống
- **Doanh Thu Hôm Nay**: Tổng giá trị bán hàng hôm nay
- **Tổng Khách Hàng**: Số lượng khách hàng đã đăng ký
- **Thuốc Sắp Hết**: Số lượng thuốc có tồn kho thấp

**Bảng Dữ Liệu:**
- **Sản Phẩm Bán Chạy Nhất**: Top 5 thuốc được bán nhiều nhất
- **Khách Hàng Hàng Đầu**: Top 5 khách hàng chi tiêu nhiều nhất
- **Đơn Hàng Gần Đây**: 5 đơn hàng mới nhất

## Quản Lý Thuốc

### 1. Xem Danh Sách Thuốc
- Vào menu bên trái → Chọn "Quản lý thuốc"
- Hiển thị bảng danh sách tất cả thuốc
- Thông tin hiển thị: Tên, nhà sản xuất, giá, tồn kho, hạn sử dụng

### 2. Thêm Thuốc Mới
- Nhấp nút "Thêm thuốc mới"
- Điền thông tin:
  - **Tên thuốc**: Nhập tên (bắt buộc)
  - **Nhà sản xuất**: Nhập tên nhà sản xuất (bắt buộc)
  - **Giá**: Nhập giá bán (bắt buộc)
  - **Tồn kho**: Nhập số lượng ban đầu (bắt buộc)
  - **Mức tối thiểu**: Nhập mức cảnh báo (bắt buộc)
  - **Hạn sử dụng**: Chọn ngày hết hạn (bắt buộc)
  - **Mô tả**: Nhập mô tả chi tiết (tùy chọn)
- Nhấp "OK" để lưu

### 3. Cập Nhật Thông Tin Thuốc
- Nhấp nút "Sửa" trên dòng thuốc cần sửa
- Chỉnh sửa thông tin cần thiết
- Nhấp "OK" để lưu

### 4. Xóa Thuốc
- Nhấp nút "Xóa" trên dòng thuốc
- Xác nhận việc xóa
- Thuốc sẽ bị ẩn khỏi danh sách

### 5. Cảnh Báo Tồn Kho
- **Màu Xanh**: Tồn kho bình thường
- **Màu Đỏ**: Tồn kho thấp (dưới mức tối thiểu)

## Quản Lý Khách Hàng

### 1. Xem Danh Sách Khách Hàng
- Vào menu → Chọn "Quản lý khách hàng"
- Hiển thị tất cả khách hàng đã đăng ký

### 2. Thêm Khách Hàng Mới
- Nhấp "Thêm khách hàng mới"
- Điền thông tin:
  - **Tên khách hàng** (bắt buộc)
  - **Số điện thoại** (bắt buộc)
  - **Email** (tùy chọn)
  - **Địa chỉ**: Số nhà, đường
  - **Tỉnh/Thành phố**: Chọn từ danh sách
  - **Quận/Huyện**: Nhập
  - **Phường/Xã**: Nhập
  - **Giới tính**: Nam/Nữ/Khác
- Nhấp "OK"

### 3. Cập Nhật Thông Tin Khách Hàng
- Nhấp "Sửa" trên dòng khách hàng
- Sửa thông tin cần thiết
- Nhấp "OK"

### 4. Xem Lịch Sử Giao Dịch
- Nhấp vào tên khách hàng hoặc "Chi tiết"
- Xem danh sách đơn hàng của khách hàng
- Xem tổng chi tiêu

### 5. Tìm Kiếm Khách Hàng
- Sử dụng thanh tìm kiếm ở đầu trang
- Nhập tên, số điện thoại, hoặc email
- Nhấp Enter hoặc nút Tìm kiếm

## Quản Lý Đơn Hàng

### 1. Xem Danh Sách Đơn Hàng
- Vào menu → "Quản lý đơn hàng"
- Hiển thị tất cả đơn hàng

### 2. Tạo Đơn Hàng Mới
- Nhấp "Tạo đơn hàng mới"
- **Chọn khách hàng**: Click vào trường, chọn từ danh sách
- **Chọn phương thức thanh toán**:
  - Tiền mặt
  - Thẻ ngân hàng
  - Chuyển khoản
- **Thêm sản phẩm**: 
  - Chọn thuốc từ danh sách
  - Nhập số lượng
  - Hệ thống tự động tính giá
- **Nhập giảm giá** (nếu có)
- **Nhập thuế** (nếu có)
- **Ghi chú** (tùy chọn)
- Nhấp "OK" để lưu đơn hàng

### 3. In Hóa Đơn
- Nhấp nút "In" trên dòng đơn hàng
- Hóa đơn sẽ hiển thị trên cửa sổ in
- Nhấp "In" trong cửa sổ browser

### 4. Cập Nhật Trạng Thái Đơn Hàng
- Chọn đơn hàng
- Cập nhật trạng thái: Pending/Completed/Cancelled

### 5. Xóa Đơn Hàng
- Nhấp "Xóa" trên dòng đơn hàng
- Xác nhận xóa

## Báo Cáo Thống Kê

### 1. Báo Cáo Doanh Thu
- Vào menu → "Báo cáo & thống kê"
- Chọn khoảng thời gian:
  - Chọn ngày bắt đầu
  - Chọn ngày kết thúc
  - Nhấp "Cập nhật"
- Xem doanh thu tổng cộng

### 2. Sản Phẩm Bán Chạy Nhất
- Xem bảng "Sản phẩm bán chạy nhất"
- Hiển thị: Tên thuốc, giá, số lượng bán

### 3. Khách Hàng Hàng Đầu
- Xem bảng "Khách hàng hàng đầu"
- Hiển thị: Tên khách hàng, tổng chi tiêu
- Sắp xếp theo chi tiêu cao nhất

### 4. Thuốc Hết Hàng
- Xem bảng "Thuốc hết hàng"
- Liệt kê các thuốc có tồn kho = 0
- Mức tối thiểu cho biết nên bao nhiêu

## Quản Lý Tồn Kho

### 1. Xem Lịch Sử Nhập/Xuất Kho
- Vào menu → "Quản lý tồn kho"
- Hiển thị lịch sử giao dịch kho

### 2. Chi Tiết Giao Dịch
- **Loại giao dịch**: Nhập/Xuất
- **Thuốc**: Tên thuốc
- **Số lượng**: Bao nhiêu đơn vị
- **Tồn kho trước**: Số lượng trước giao dịch
- **Tồn kho sau**: Số lượng sau giao dịch
- **Lý do**: Nhập hàng/Bán hàng/Điều chỉnh
- **Ngày giao dịch**: Thời gian

### 3. Lọc Theo Khoảng Thời Gian
- Chọn ngày bắt đầu
- Chọn ngày kết thúc
- Nhấp "Lọc"

### 4. Tìm Kiếm Lịch Sử
- Nhập tên thuốc
- Hệ thống tìm kiếm tất cả giao dịch của thuốc đó

## Các Tính Năng Khác

### 1. Tìm Kiếm Toàn Hệ Thống
- Sử dụng thanh tìm kiếm ở mỗi trang
- Nhập từ khóa
- Kết quả hiển thị tức thời

### 2. Quản Lý Tài Khoản
- Nhấp vào avatar góc trên bên phải
- Chọn "Tài khoản của tôi"
- Xem/cập nhật thông tin cá nhân

### 3. Đăng Xuất
- Nhấp vào avatar
- Chọn "Đăng xuất"
- Quay lại trang đăng nhập

### 4. Thông Báo
- Biểu tượng chuông ở góc trên bên phải
- Hiển thị số thông báo chưa đọc
- Nhấp để xem danh sách

## Xử Lý Sự Cố

### Vấn Đề: Không Thể Đăng Nhập
**Giải Pháp:**
- Kiểm tra kết nối internet
- Xóa cache trình duyệt
- Thử trình duyệt khác
- Kiểm tra backend đang chạy: `http://localhost:5000/swagger`

### Vấn Đề: Dữ Liệu Không Hiển Thị
**Giải Pháp:**
- Làm mới trang (F5)
- Kiểm tra kết nối mạng
- Xem console (F12) để tìm lỗi

### Vấn Đề: Không Thể Tạo Đơn Hàng
**Giải Pháp:**
- Kiểm tra khách hàng đã tồn tại
- Kiểm tra tồn kho thuốc
- Kiểm tra tất cả trường bắt buộc đã điền
- Xem console để tìm lỗi chi tiết

### Vấn Đề: Dữ Liệu Mất Sau Khi Tạo
**Giải Pháp:**
- Backend có thể đã bị ngắt
- Khởi động lại backend: `dotnet run` trong folder Backend
- Tạo lại dữ liệu

## Tips & Tricks

### Làm Việc Hiệu Quả
1. **Tạo Khách Hàng Trước**: Tạo danh sách khách hàng thường xuyên trước
2. **Cập Nhật Tồn Kho Định Kỳ**: Kiểm tra tồn kho mỗi ngày
3. **Sử Dụng Tìm Kiếm**: Tìm kiếm nhanh thay vì cuộn danh sách dài
4. **Backup Định Kỳ**: Yêu cầu backup database thường xuyên

### Quản Lý Dữ Liệu
1. **Giá Thuốc**: Cập nhật kịp thời khi có thay đổi
2. **Tồn Kho Tối Thiểu**: Đặt hợp lý để tránh hết hàng
3. **Khách Hàng Thường Xuyên**: Lưu thông tin chi tiết
4. **Đơn Hàng Cũ**: Lưu trữ để tracking doanh thu

## Liên Hệ Hỗ Trợ

Nếu cần hỗ trợ:
- Kiểm tra phần "Xử Lý Sự Cố" ở trên
- Liên hệ với bộ phận IT/Quản lý hệ thống
- Cung cấp ảnh chụp lỗi khi báo cáo vấn đề

---

**Cập Nhật Lần Cuối**: December 2024  
**Phiên Bản**: 1.0.0
