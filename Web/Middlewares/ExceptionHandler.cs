using Domain.DomainExceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.Middlewares;

public class ExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService problemDetailsService;

    public ExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        this.problemDetailsService = problemDetailsService;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetail = new ProblemDetails();

        if (exception is UserNotFoundException)
        {
            problemDetail.Status = 404;
            problemDetail.Title = "Not found.";
        }
        else if (exception is AgeNotAllowedException or UserNameTooLongException or ValidationException)
        {
            problemDetail.Status = 400;
            problemDetail.Title = "Bad Request";
        }
        else
        {
            problemDetail.Status = 500;
            problemDetail.Title = "An error occurred.";
        }

        problemDetail.Detail = exception.Message;

        var problemContext = new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetail
        };

        await problemDetailsService.TryWriteAsync(problemContext);
        return true;
    }
}
