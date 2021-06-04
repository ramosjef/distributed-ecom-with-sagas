using System.Diagnostics.CodeAnalysis;

namespace Checkout.Infrastructure.DataAccess.Mongo;

[ExcludeFromCodeCoverage]
public class MongoConfiguration
{
    public string Connection { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public string Server { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
}
