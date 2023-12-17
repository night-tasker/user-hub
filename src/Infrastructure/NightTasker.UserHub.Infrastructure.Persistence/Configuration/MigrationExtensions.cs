using Microsoft.EntityFrameworkCore;

namespace NightTasker.UserHub.Infrastructure.Persistence.Configuration;

public static class MigrationExtensions
{
    /// <summary>
    /// Применение миграций базы данных.
    /// </summary>
    /// <param name="context">Контекст БД.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public static async Task MigrateAsync(
        this ApplicationDbContext context, 
        CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken);
        }
    } 
}