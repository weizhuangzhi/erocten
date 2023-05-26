namespace Erocten.Foundation.EntityFrameworkCore.DatabaseProvider;

public class PostgreSqlDatabaseProvider : IDatabaseTypeProvider
{
    public string ProviderName => nameof(PostgreSqlDatabaseProvider);
}