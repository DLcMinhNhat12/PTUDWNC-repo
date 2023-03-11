using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

var builder = WebApplication.CreateBuilder(args);
{
    // Thêm các dịch vụ được yêu cầu bởi MVC Framework
    builder.Services.AddControllersWithViews();

    // Đăng ký dịch vụ với DI Container
    builder.Services.AddDbContext<BlogDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IBlogRepository, BlogRepository>();
    //builder.Services.AddScoped<BlogDbContext>();
    builder.Services.AddScoped<IDataSeeder, DataSeeder>();
}

var app = builder.Build();

// Config Http Request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Blog/Error");

    // Thêm middleware cho việc áp dụng HSTS (thêm header
    // Strict-Transport-Security vào HTTP Response).
    app.UseHsts();
}

// Thêm middleware để chuyển hướng HTTP -> HTTPS
app.UseHttpsRedirection();

// Thêm middleware phục vụ các yêu cầu liên quan
// tới các tập tin nội dung tĩnh như images, css...
app.UseStaticFiles();

// Thêm middleware lựa chọn endpoint phù hợp nhất
// để xử lý 1 HTTP request.
app.UseRouting();

// Định nghĩa route template, route constraint cho các
// endpoints kết hợp với các action trong các controller.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blog}/{action=Index}/{id?}");


// Thêm dữ liệu mẫu vào CSLD
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    seeder.Initialize();
}

app.Run();