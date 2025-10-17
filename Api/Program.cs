using Domain.ValueObjects;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var appSetting = builder.Configuration.GetSection("appsetting").Get<AppSetting>();
var baseAccount = builder.Configuration.GetSection("BaseAccount").Get<BaseAccount>();

// Thêm dịch vụ vào container.

// Đăng ký database
builder.Services.RegisterDb(builder.Configuration, appSetting);

// Thêm Dependency Injection
builder.Services.AddDependencyInjection();

// Thêm Auto Mapper.
builder.Services.AddAutoMapperConfiguration();

// Thêm các repository.
builder.Services.AddRepositories();

builder.Services.AddControllers();

// Tìm hiểu thêm về cách cấu hình OpenAPI tại https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.AutoMigrations().GetAwaiter().GetResult();

app.SeedData(builder.Configuration, baseAccount).GetAwaiter().GetResult();

// Cấu hình đường dẫn HTTP request.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Sử dụng Swagger UI để xem API.
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
