using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project3.Authorization;
using Project3.Models;
using Project3.ModelsView;
using Project3.ModelsView.Identity;
using Project3.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Configure Serilog

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
// sử dụng 1 chuỗi chung cho EcommerceContext và ApplicationDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<VehicleInsuranceManagementContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));


// Register Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
// Configure application cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/";
});


////Cấu hình quyền truy cập
builder.Services.AddAuthorization(options =>
{
    //    // Chính sách cho Admin
    options.AddPolicy("RequireAdminRole", policy =>
    {
        policy.RequireRole("Admin");
    });

    // Chính sách cho ProductManager
    options.AddPolicy("AdminInsuranceProducts", policy =>
    {
        policy.Requirements.Add(new PermissionRequirement("AdminInsuranceProducts"));
    });
    options.AddPolicy("AdminEstimates", policy =>
    {
        policy.Requirements.Add(new PermissionRequirement("AdminEstimates"));
    });
    options.AddPolicy("AdminVehicleInformations", policy =>
    {
        policy.Requirements.Add(new PermissionRequirement("AdminVehicleInformations"));
    });
    options.AddPolicy("AdminCompanyBillingPolicies", policy =>
    {
        policy.Requirements.Add(new PermissionRequirement("AdminCompanyBillingPolicies"));
    });
    options.AddPolicy("AdminInsuranceProcess", policy =>
    {
        policy.Requirements.Add(new PermissionRequirement("AdminInsuranceProcess"));
    });
    options.AddPolicy("AdminContactUs", policy =>
    {
        policy.Requirements.Add(new PermissionRequirement("AdminContactUs"));
    });
    options.AddPolicy("AdminAccount", policy =>
    {
        policy.Requirements.Add(new PermissionRequirement("AdminAccount"));
    });

    // chặn user truy cập vào System area
    options.AddPolicy("AuthorizeSystemAreas", policy =>
    {
        policy.RequireAssertion(context =>
        context.User.Identity.IsAuthenticated && // Kiểm tra người dùng đã đăng nhập hay chưa qua identity
            !context.User.IsInRole("User") // Người dùng thường không được phép
            || context.User.IsInRole("Admin") // Admin vẫn được phép
            || context.User.HasClaim(c => c.Type == "AdminInsuranceProducts" && c.Value == "True") // ProductManager vẫn được phép
            || context.User.HasClaim(c => c.Type == "AdminEstimates" && c.Value == "True") // AccountManager vẫn được phép
            || context.User.HasClaim(c => c.Type == "AdminVehicleInformations" && c.Value == "True")
            || context.User.HasClaim(c => c.Type == "AdminCompanyBillingPolicies" && c.Value == "True")
            || context.User.HasClaim(c => c.Type == "AdminInsuranceProcess" && c.Value == "True")
            || context.User.HasClaim(c => c.Type == "AdminContactUs" && c.Value == "True")
            || context.User.HasClaim(c => c.Type == "AdminAccount" && c.Value == "True")
        );
    });
});


<<<<<<< HEAD
//đăng ký login google
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    //options.DefaultScheme = GoogleDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    //clientId và ClientSecret  dc cấu hình ở appsettings.json
    options.ClientId = builder.Configuration["GoogleKeys:ClientId"];
    options.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"];
});
=======
////đăng ký login google
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//    //options.DefaultScheme = GoogleDefaults.AuthenticationScheme;
//    //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//})
//.AddCookie()
//.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
//{
//    //clientId và ClientSecret  dc cấu hình ở appsettings.json
//    options.ClientId = builder.Configuration["GoogleKeys:ClientId"];
//    options.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"];
//});
>>>>>>> bb36989ac0503e71612aac8540c2b3acd19250fb


// Add services to the container.
// Register HttpClient for MomoPaymentService
builder.Services.AddHttpClient<MomoPaymentService>();

// Register IPaymentService with MomoPaymentService
builder.Services.AddScoped<IPaymentService, MomoPaymentService>();

//load thông tin cấu hình và lưu vào đối tượng MailSetting
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
//add dependency inject cho MailService
builder.Services.AddTransient<Project3.ModelsView.IMailService, Project3.ModelsView.MailService>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UserRoleService, UserRoleService>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddTransient<IEmail, Email>();
// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<BillingCalculationService>();
builder.Services.AddSingleton<CarService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

// Create roles and admin user on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();//ApplicationUser
    var context = scope.ServiceProvider.GetRequiredService<VehicleInsuranceManagementContext>();
    await CreateRolesAndAdminUser(roleManager, userManager, context);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();

// Method to create roles and admin user
async Task CreateRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, VehicleInsuranceManagementContext context)
{
    string[] roleNames = { "Admin", "User", "AdminAccount", "AdminEstimates", "AdminInsuranceProcess", "AdminInsuranceProducts", "AdminVehicleInformations", "AdminCompanyBillingPolicies", "AdminContactUs" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            var role = new IdentityRole(roleName);
            var roleResult = await roleManager.CreateAsync(role);

            if (roleResult.Succeeded)
            {
                Console.WriteLine($"Role '{roleName}' created successfully.");

                // Nếu role là Admin hoặc User, thêm vào bảng NameRole với cùng ID
                if (roleName == "Admin" || roleName == "User")
                {
                    var existingNameRole = context.NameRoles.SingleOrDefault(r => r.Id == role.Id);
                    if (existingNameRole == null)
                    {
                        context.NameRoles.Add(new NameRole { Id = role.Id, NameRole1 = role.Name });
                        await context.SaveChangesAsync(); // Lưu thay đổi ngay lập tức
                        Console.WriteLine($"Role '{roleName}' added to NameRole table successfully.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Failed to create role '{roleName}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }
    }

    // Tạo admin user mặc định nếu chưa tồn tại
    var admin = new ApplicationUser
    {
        UserName = "huy2010@gmail.com",
        Email = "huy2010@gmail.com",
        Fullname = "Quang Huy",
        Phone = "0123456789",
        EmailConfirmed = true
    };

    string adminPassword = "123456h";
    var _admin = await userManager.FindByEmailAsync("huy2010@gmail.com");

    if (_admin == null)
    {
        var createAdmin = await userManager.CreateAsync(admin, adminPassword);
        if (createAdmin.Succeeded)
        {
            Console.WriteLine("Admin user created successfully.");
            var addToRoleResult = await userManager.AddToRoleAsync(admin, "Admin");
            if (addToRoleResult.Succeeded)
            {
                Console.WriteLine("Admin user added to 'Admin' role successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to add admin to 'Admin' role: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            Console.WriteLine($"Failed to create admin user: {string.Join(", ", createAdmin.Errors.Select(e => e.Description))}");
        }
    }
    else
    {
        Console.WriteLine("Admin user already exists.");
    }
}

