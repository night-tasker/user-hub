namespace NightTasker.UserHub.Infrastructure.Grpc.Settings;

public abstract class GrpcSettings
{
    public string? Host { get; set; }

    public string? Port { get; set; }
}