using Mediator;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public class LoggingPipeline<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse> where
TMessage : IMessage
{
    private readonly ILogger<LoggingPipeline<TMessage, TResponse>> logger;

    public LoggingPipeline(ILogger<LoggingPipeline<TMessage, TResponse>> logger)
    {
        this.logger = logger;
    }
    public async ValueTask<TResponse> Handle(TMessage message, MessageHandlerDelegate<TMessage, TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request received at {method} on {datetime}", typeof(TMessage).Name, DateTime.UtcNow);

        var result = await next(message, cancellationToken);

        logger.LogInformation("Request completed at {methodName} at {datetime}", typeof(TMessage).Name, DateTime.UtcNow);

        return result;
    }
}
