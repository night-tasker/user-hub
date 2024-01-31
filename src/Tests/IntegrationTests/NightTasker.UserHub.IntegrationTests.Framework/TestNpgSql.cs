using Testcontainers.PostgreSql;

namespace NightTasker.UserHub.IntegrationTests.Framework;

public class TestNpgSql
{
    public readonly PostgreSqlContainer NpgSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16")
        .Build();

    public TestNpgSql()
    {
        NpgSqlContainer.StartAsync().GetAwaiter().GetResult();
    }
}