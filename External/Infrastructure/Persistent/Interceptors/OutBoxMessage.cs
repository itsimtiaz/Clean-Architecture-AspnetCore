namespace Persistent.Interceptors;

internal class OutBoxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime DateTimeOccurredOn { get; set; }
    public DateTime? DateTimeProcessedOn { get; set; }
    public string? Error { get; set; }
}
