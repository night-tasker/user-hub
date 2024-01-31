namespace NightTasker.UserHub.Infrastructure.Messaging.Settings;

public class RabbitMqSettings
{
    public string? Host { get; set; }

    public string? VirtualHost { get; set; }

    public string? Port { get; set; }
    
    public string? Username { get; set; }

    public string? Password { get; set; }
}