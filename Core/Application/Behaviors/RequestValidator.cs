using FluentValidation;
using Mediator;

namespace Application.Behaviors;

public class RequestValidator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidator(IEnumerable<IValidator<TRequest>> validator)
    {
        _validators = validator;
    }

    public async ValueTask<TResponse> Handle(TRequest message, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        var results = await Task.WhenAll(_validators.Select(_ => _.ValidateAsync(message, cancellationToken)));

        if (results.Any(e => !e.IsValid))
        {
            var errorMessages = results.SelectMany(_ => _.Errors).Select(_ => _.ErrorMessage);

            throw new ValidationException(string.Join(", ", errorMessages));
        }

        return await next(message, cancellationToken);
    }
}
