var builder = WebApplication.CreateBuilder(args);

// 设置Server标头不包含在每个响应中。
builder.WebHost.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.AddSession();

// 设置Web编码服务配置中允许由编码器以非转义形式表示的码位。
builder.Services.AddWebEncoders(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

builder.Services.AddDbContext<CommonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultContext") ?? throw new InvalidOperationException("Connection string 'DefaultContext' not found.")));


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
