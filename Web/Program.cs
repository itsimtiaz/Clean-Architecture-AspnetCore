using Application;
using Infrastructure;
using Persistent;
using Presentation;
using Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddPersistent(builder.Configuration)
    .AddInfrastructure()
    .AddPresentation();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

app.AddEndPoints();

app.Run();
