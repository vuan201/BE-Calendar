using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Domain.constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.DataAccess;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Infrastructure;

public static class Configuration
{
    // * Đăng ký DbContext và cấu hình kết nối cơ sở dữ liệu
    public static void RegisterDb(this IServiceCollection service, IConfiguration confix)
    {
        var appSetting = confix.GetSection("appsetting").Get<AppSetting>();

        // * Đăng ký DbContext và cấu hình kết nối cơ sở dữ liệu
        service.AddDbContext<ApplicationDbContext>(options =>
        {
            // * Sử dụng SqlServer với chuỗi kết nối và phiên bản của SqlServerServerVersion
            options.UseSqlServer(appSetting?.Connections?.SqlServerConnectionString);

            // * Cho phép sử dụng lazy loading proxies để tải các đối tượng liên quan 
            options.UseLazyLoadingProxies();
        });

        // * Cấu hình dịch vụ Identity
        service.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // * Không yêu cầu xác thực email
            options.SignIn.RequireConfirmedEmail = false;

            // * Không yêu cầu xác thực số điện thoại
            options.SignIn.RequireConfirmedPhoneNumber = false;

            // * Không yêu cầu xác thực tài khoản
            options.SignIn.RequireConfirmedAccount = false;

            // * Cấu hình chính sách mật khẩu (tùy chọn)

            // * Yêu cầu có số
            options.Password.RequireDigit = true;

            // * Yêu cầu có chữ thường
            options.Password.RequireLowercase = true;

            // * Yêu cầu có chữ hoa
            options.Password.RequireUppercase = true;

            // * Yêu cầu có ký tự đặc biệt
            options.Password.RequireNonAlphanumeric = true;

            // * Độ dài tối thiểu
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // * Cấu hình các tùy chọn về cookie cho xác thực
        service.ConfigureApplicationCookie(options =>
        {
            // * Cho phép kéo dài thời gian sống của cookie khi người dùng tiếp tục truy cập
            options.SlidingExpiration = true;

            // * Thời gian hết hạn cookie là 30 ngày
            options.ExpireTimeSpan = TimeSpan.FromDays(3);

            // * Tên Cookie
            options.Cookie.Name = "BookSaleManagermentCookie";

            // * Đường dẫn đến trang đăng nhập
            options.LoginPath = "/Admin/Authentication/Login";

            // * Cho phép kéo dài thời gian sống của cookie khi người dùng tiếp tục truy cập
            options.SlidingExpiration = true;

            // * Đường dẫn đến trang không được phép truy cập
            // * options.AccessDeniedPath = "/"; 
        });

        // * Cấu hình các tùy chọn khác về xác thực và bảo mật
        service.Configure<IdentityOptions>(options =>
        {
            // * Cho phép khóa tài khoản cho người dùng mới tạo
            options.Lockout.AllowedForNewUsers = true;

            // * Thời gian khóa mặc định là 30 giây
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);

            // * Số lần thử sai tối đa trước khi khóa tài khoản
            options.Lockout.MaxFailedAccessAttempts = 5;

            // * Các ký tự hợp lệ trong tên người dùng
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

            // * Email phải duy nhất
            options.User.RequireUniqueEmail = true;
        });
    }

    public static async Task AutoMigrations(this WebApplication webApplication)
    {
        using (var scope = webApplication.Services.CreateScope())
        {
            var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await appContext.Database.MigrateAsync();
        }
    }
    // * Cấu hình JWT Authentication
    public static void AddAuthenticationAndToken(this IServiceCollection services, IConfiguration confix)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;

            // * Nếu là true, chỉ chấp nhận request qua HTTPS
            //options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                // * Đảm bảo token phát hành bởi server hợp lệ
                ValidateIssuer = true,

                // * Đảm bảo token được dùng đúng đối tượng
                ValidateAudience = false, // ! Cho phép tất cả audience
                // * kiểm tra thời hạn
                ValidateLifetime = true,

                // * Kiểm tra khóa bí mật của token
                ValidateIssuerSigningKey = true,

                // * Token chỉ hợp lệ khi được phát hành cho các client có trong ValidAudiences
                // ValidAudiences = confix.GetSection("JWT:ValidAudiences").Get<string[]>(),

                // * Token chỉ hợp lệ khi được phát hành bởi ValidIssuer
                ValidIssuer = confix["JWT:ValidIssuer"],

                // * Khóa bí mật của token
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(confix["JWT:Secret"] ?? ""))
            };
        });
    }

    // * Xây dựng dữ liệu ban đầu cho ứng dụng, đảm bảo dữ liệu dù xóa hết cũng sẽ có dữ liệu mặc định
    public static async Task SeedData(this WebApplication webApplication, IConfiguration confix)
    {
        var roles = Roles.GetRoles();
        var baseUser = confix.GetSection("BaseAccount").Get<BaseAccount>();

        using (var scope = webApplication.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                // * Thêm tất cả các role trong code vào db nếu chưa có 
                // * Tự động hóa đồng bộ khi chỉ cần thêm role vào code
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }
                }

                if (baseUser != null)
                {
                    // * Kiểm tra xem người dùng có tồn tại hay không, nếu không thì tạo mới
                    var existUser = await userManager.FindByNameAsync(baseUser.UserName ?? string.Empty);
                    if (existUser == null)
                    {
                        // * Tạo người dùng mặc định
                        var user = new ApplicationUser
                        {
                            UserName = baseUser.UserName,
                            FirstName = baseUser.FirstName,
                            LastName = baseUser.LastName,
                            Email = baseUser.Email,
                            NormalizedEmail = baseUser.Email,
                            Address = baseUser.Address,
                            IsActive = baseUser.IsActive,
                            AccessFailedCount = 0,
                            PhoneNumber = baseUser.PhoneNumber,
                        };

                        // * thiết lập user mạc định
                        var identityUser = await userManager.CreateAsync(user, baseUser.Password);

                        if (identityUser.Succeeded)
                        {
                            // * Thêm role cho người dùng mặc định
                            await userManager.AddToRoleAsync(user, Roles.SupperAdmin);
                        }

                        // * Xác nhận email của người dùng mặc định
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        await userManager.ConfirmEmailAsync(user, token);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    // * Thêm Dependency Injection 
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        // * Đăng ký ITokenService (Dịch vụ tạo/quản lý token)
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
    }
    // * Thêm Repositories
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
    }
    // * Đăng ký AutoMapper
    public static void AddAutoMapperConfiguration(this IServiceCollection service)
    {
        service.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
    }
    // * Đăng ký Newtonsoft Json
    public static void AddNewtonSoftJson(this IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            // * Bỏ vòng lặp tuần hoàn (quan trọng khi có navigation)
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // * Ẩn các trường null
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });

    }
}