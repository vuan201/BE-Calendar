# BE-Calendar - Hệ thống Quản lý Lịch (Backend)

Dự án này cung cấp các API mạnh mẽ để quản lý sự kiện và lịch cá nhân, được xây dựng theo kiến trúc **Clean Architecture** để đảm bảo tính mở rộng và dễ bảo trì.

## Các chức năng chính
- **Xác thực và Phân quyền**: Đăng ký, đăng nhập và quản lý phiên làm việc sử dụng **JWT (JSON Web Token)** và **ASP.NET Core Identity**.
- **Quản lý Sự kiện (Events)**: Cung cấp đầy đủ các thao tác CRUD (Thêm, Xóa, Sửa, Đọc) cho các sự kiện trong lịch.
- **Quản lý Người dùng**: Lưu trữ và quản lý thông tin hồ sơ người dùng.
- **Xử lý Lỗi Tập trung**: Middleware tùy chỉnh để quản lý ngoại lệ và trả về phản hồi đồng nhất.
- **Tự động Migrations & Seeding**: Tự động cập nhật cấu trúc cơ sở dữ liệu và nạp dữ liệu mẫu khi khởi chạy ứng dụng.

## Công nghệ sử dụng
- **Nền tảng**: .NET 9.0 (ASP.NET Core Web API)
- **Cơ sở dữ liệu**: SQL Server
- **ORM**: Entity Framework Core 9.0
- **Bảo mật**: ASP.NET Core Identity, JWT Bearer Authentication
- **Mapping**: AutoMapper
- **Thư viện JSON**: Newtonsoft.Json
- **Tài liệu API**: OpenAPI / Swagger UI

## Yêu cầu tiên quyết
- **.NET SDK 9.0**: Cài đặt SDK để build và chạy solution.
- **Instance SQL Server**: Chuẩn bị thông tin kết nối cho cơ sở dữ liệu của ứng dụng.
- **Công cụ Entity Framework Core**: Nếu chưa có, cài đặt toàn cục bằng `dotnet tool install --global dotnet-ef`.

## Các bước thực hiện
1. **Khôi phục phụ thuộc**
   ```bash
   dotnet restore
   ```
2. **Tạo tệp cấu hình ứng dụng**
   Sao chép `Api/appsettings.template.json` thành `Api/appsettings.json` và `Api/appsettings.Development.json`, sau đó cập nhật các giá trị cấu hình cho phù hợp với môi trường của bạn.

   **Lưu ý quan trọng**: Không commit các tệp `appsettings.json` và `appsettings.Development.json` vào git vì chúng chứa thông tin nhạy cảm. Các tệp này đã được thêm vào `.gitignore`.
3. **Tạo migration khởi đầu**
   ```bash
   dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Api --output-dir DataAccess/Migrations
   ```
4. **Cập nhật schema cơ sở dữ liệu**
   ```bash
   dotnet ef database update --project Infrastructure --startup-project Api
   ```
5. **Kiểm tra cấu hình DbContext**
   ```bash
   dotnet ef dbcontext info --project Infrastructure --startup-project Api
   ```
6. **Chạy dự án API**
   ```bash
   dotnet run --project Api
   ```
