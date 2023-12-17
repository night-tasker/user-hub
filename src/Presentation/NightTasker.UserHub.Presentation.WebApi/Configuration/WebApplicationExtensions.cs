using Microsoft.EntityFrameworkCore;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.Infrastructure.Persistence.Configuration;

namespace NightTasker.UserHub.Presentation.WebApi.Configuration;

public static class WebApplicationExtensions
{
    public static async Task ApplyDatabaseMigrations(
        this WebApplication app,
        CancellationToken cancellationToken)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            await context.MigrateAsync(cancellationToken);
        }
    }
}