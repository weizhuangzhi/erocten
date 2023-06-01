var builder = WebApplication.CreateBuilder(args);

// 设置Server标头不包含在每个响应中。
builder.WebHost.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.AddSession();

// 设置Web编码服务配置中允许由编码器以非转义形式表示的码位。
builder.Services.AddWebEncoders(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

builder.Services.AddDbContext<CommonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultContext") ?? throw new InvalidOperationException("Connection string 'DefaultContext' not found.")));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(ApplicationPartUtilities.GetApplicationPartAssemblies(builder.Environment.ApplicationName).ToArray());
    cfg.NotificationPublisher = new SortedNotificationPublisher();
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CommonDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseForwardedHeaders();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();
app.UseRateLimiter();
app.UseRequestLocalization();
app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

// app.UseResponseCompression();

app.UseResponseCaching();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
