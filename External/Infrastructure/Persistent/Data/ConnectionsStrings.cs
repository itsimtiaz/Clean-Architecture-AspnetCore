namespace Persistent.Data;

internal class DatabaseConnections
{
    public const string ConnectionStrings = "ConnectionStrings";
    public string DefaultConnection { get; set; } = null!;
}
