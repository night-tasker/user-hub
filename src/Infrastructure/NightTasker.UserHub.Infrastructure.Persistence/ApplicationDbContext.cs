using Microsoft.EntityFrameworkCore;
using NightTasker.Common.Core.Abstractions;
using NightTasker.Common.Core.Extensions;

namespace NightTasker.UserHub.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        SavingChanges += OnSavingChanges;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        UseSnakeCaseNamingConventions(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    
    private static void UseSnakeCaseNamingConventions(ModelBuilder builder)
    {
        foreach(var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()?.ConvertCamelToSnakeCase());
            
            foreach(var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ConvertCamelToSnakeCase());
            }

            foreach(var key in entity.GetKeys())
            {
                key.SetName(key.GetName()?.ConvertCamelToSnakeCase());
            }

            foreach(var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(key.GetConstraintName()?.ConvertCamelToSnakeCase());
            }

            foreach(var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()?.ConvertCamelToSnakeCase());
            }
        }
    }

    private void OnSavingChanges(object? sender, SavingChangesEventArgs args)
    {
        ConfigureEntityDates();
    }
    
    private void ConfigureEntityDates()
    {
        var updatedEntities = ChangeTracker.Entries()
            .Where(x => x is { Entity: IUpdatedDateTimeOffset, State: EntityState.Modified })
            .Select(x => x.Entity as IUpdatedDateTimeOffset);

        foreach (var entity in updatedEntities)
        {
            if (entity is not null)
            {
                entity.UpdatedDateTimeOffset = DateTimeOffset.Now;
            }
        }

        var createdEntities = ChangeTracker.Entries()
            .Where(x => x is { Entity: ICreatedDateTimeOffset, State: EntityState.Added })
            .Select(x => x.Entity as ICreatedDateTimeOffset);

        foreach (var entity in createdEntities)
        {
            if (entity is not null)
            {
                entity.CreatedDateTimeOffset = DateTimeOffset.Now;
            }
        }
    }
}