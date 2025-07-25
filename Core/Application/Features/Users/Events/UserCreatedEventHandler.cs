using Application.Interfaces;
using Domain.Events;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Events;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly IEmailHandler emailHandler;
    private readonly ILogger<UserCreatedEventHandler> logger;

    public UserCreatedEventHandler(IEmailHandler emailHandler, ILogger<UserCreatedEventHandler> logger)
    {
        this.emailHandler = emailHandler;
        this.logger = logger;
    }
    public async ValueTask Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("User has created successfully.");

        await emailHandler.SendAsync("User.Name", "Your account has been successfully created.");
    }
}
