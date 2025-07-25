using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

internal class EmailHandler : IEmailHandler
{
    private readonly ILogger<EmailHandler> logger;

    public EmailHandler(ILogger<EmailHandler> logger)
    {
        this.logger = logger;
    }

    public Task SendAsync(string to, string message)
    {
        logger.LogInformation("Sending email to {to}, with message {message}", to, message);
        return Task.CompletedTask;
    }

}
