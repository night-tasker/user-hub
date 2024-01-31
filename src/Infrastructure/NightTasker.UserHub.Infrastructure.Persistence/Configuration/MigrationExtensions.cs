using Microsoft.EntityFrameworkCore;

namespace NightTasker.UserHub.Infrastructure.Persistence.Configuration;

public static class MigrationExtensions
{
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