namespace Application.Interfaces;

public interface IEmailHandler
{
    Task SendAsync(string to, string message);
}
