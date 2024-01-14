namespace NightTasker.UserHub.Infrastructure.Grpc.Settings;

/// <summary>
/// Настройки для gRPC.
/// </summary>
public abstract class GrpcSettings
{
    /// <summary>
    /// Адрес хоста.
    /// </summary>
    public string? Host { get; set; }
    
    /// <summary>
    /// Порт.
    /// </summary>
    public string? Port { get; set; }
}