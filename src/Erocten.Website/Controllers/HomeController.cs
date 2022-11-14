namespace Erocten.Website.Controllers;

/// <summary>
/// Home 控制器。
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    /// <summary>
    /// 初始化 HomeController 类的新实例。
    /// </summary>
    /// <param name="logger">日志记录器。</param>
    public HomeController(ILogger<HomeController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// 首页操作方法。
    /// </summary>
    /// <returns>操作方法结果。</returns>
    public IActionResult Index()
    {
        return this.View();
    }

    /// <summary>
    /// 隐私操作方法。
    /// </summary>
    /// <returns>操作方法结果。</returns>
    public IActionResult Privacy()
    {
        return this.View();
    }

    /// <summary>
    /// 错误操作方法。
    /// </summary>
    /// <returns>操作方法结果。</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}