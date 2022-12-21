namespace Erocten.Foundation.EntityFrameworkCore;

public class PostgreSqlDatabaseProvider : IDatabaseProvider
{
    public string ProviderName => nameof(PostgreSqlDatabaseProvider);
}