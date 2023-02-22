using Erocten.Foundation.Abstractions.Notification;
using MediatR;

namespace Erocten.Modules.Common.Handler;

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
