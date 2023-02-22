namespace Erocten.Foundation.EntityFrameworkCore.DatabaseProvider;

public class PostgreSqlDatabaseProvider : IDatabaseProvider
{
    public string ProviderName => nameof(PostgreSqlDatabaseProvider);
}