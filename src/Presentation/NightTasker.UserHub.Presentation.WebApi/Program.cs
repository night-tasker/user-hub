using NightTasker.UserHub.Core.Application.Configuration;
using NightTasker.UserHub.Infrastructure.Messaging.Configuration;
using NightTasker.UserHub.Infrastructure.Persistence.Configuration;
using NightTasker.UserHub.Presentation.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .RegisterApplicationServices()
    .RegisterPersistenceServices(builder.Configuration)
    .RegisterMessagingServices(builder.Configuration)
    .RegisterApiServices();

var app = builder.Build();

await app.ApplyDatabaseMigrations(CancellationToken.None);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
