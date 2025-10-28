using Domain.ValueObjects;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// *Thêm dịch vụ vào container.
// *Đăng ký database
builder.Services.RegisterDb(builder.Configuration);

// *Cấu hình xác thực.
builder.Services.AddAuthenticationAndToken(builder.Configuration);

// *Thêm Dependency Injection
builder.Services.AddDependencyInjection();

// *Thêm Auto Mapper.
builder.Services.AddAutoMapperConfiguration();

// *Thêm các repository.
builder.Services.AddRepositories();

builder.Services.AddControllers();

// *Tìm hiểu thêm về cách cấu hình OpenAPI tại https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// *Sử dụng Newtonsoft.Json để serialize/deserialize JSON thay cho System.Text.Json.
builder.Services.AddNewtonSoftJson();
builder.Services.AddHttpContextAccessor();

builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

app.AutoMigrations().GetAwaiter().GetResult();

app.SeedData(builder.Configuration).GetAwaiter().GetResult();

// *Cấu hình đường dẫn HTTP request.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // *Sử dụng Swagger UI để xem API.
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
