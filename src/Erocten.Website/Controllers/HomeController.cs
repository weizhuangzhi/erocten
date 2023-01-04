using Erocten.Foundation.Abstractions.Notification;

namespace Erocten.Website.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly IMediator mediator;

    public HomeController(ILogger<HomeController> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        await this.mediator.Publish(new AccessNotification());
        return this.View();
    }

    public IActionResult Privacy()
    {
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}