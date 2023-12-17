namespace NightTasker.Identity.Infrastructure.Messaging.Settings;

/// <summary>
/// Настройки подключения к RabbitMQ.
/// </summary>
public class RabbitMqSettings
{
    /// <summary>
    /// Хост.
    /// </summary>
    public string? Host { get; set; }

    /// <summary>
    /// VirtualHost.
    /// </summary>
    public string? VirtualHost { get; set; }

    /// <summary>
    /// Порт.
    /// </summary>
    public string? Port { get; set; }
    
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public string? Password { get; set; }
}