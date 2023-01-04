using Erocten.Foundation.Abstractions.Notification;

namespace Erocten.Website.MediatR;

public class AccessNotificationHandler : INotificationHandler<AccessNotification>
{
    private readonly ILogger<AccessNotificationHandler> logger;

    public AccessNotificationHandler(ILogger<AccessNotificationHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(AccessNotification notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Access Action.");
        return Task.CompletedTask;
    }
}
