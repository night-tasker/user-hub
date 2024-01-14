using NightTasker.Common.Core.Exceptions.Middlewares;
using NightTasker.UserHub.Core.Application.Configuration;
using NightTasker.UserHub.Infrastructure.Grpc.Configuration;
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
    .RegisterGrpcServices(builder.Configuration)
    .RegisterApiServices(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

await app.ApplyDatabaseMigrations(CancellationToken.None);

app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
