using Testcontainers.PostgreSql;

namespace NightTasker.UserHub.IntegrationTests.Framework;

public class TestNpgSql
{
    public readonly PostgreSqlContainer NpgSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16")
        .WithCleanUp(true)
        .WithDatabase("night-tasker")
        .Build();

    public TestNpgSql()
    {
        NpgSqlContainer.StartAsync().GetAwaiter().GetResult();
    }

    public void DropDatabase()
    {
        NpgSqlContainer.ExecScriptAsync("DROP SCHEMA public CASCADE;").GetAwaiter().GetResult();
        NpgSqlContainer.ExecScriptAsync("CREATE SCHEMA public;").GetAwaiter().GetResult();
    }
}