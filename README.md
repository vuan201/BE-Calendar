# Hướng dẫn thiết lập WebApi

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
   Tạo hoặc cập nhật `Api/appsettings.json` với chuỗi kết nối SQL Server của bạn.
   ```json
   {
     "AppSetting": {
       "Connections": {
         "SqlServerConnectionString": "Server=localhost;Database=CalendarApi;Uid=sa;Pwd=123456;Trusted_Connection=True"
       }
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*"
   }
   ```
   Điều chỉnh `SqlServerConnectionString` cho phù hợp với môi trường của bạn.
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
