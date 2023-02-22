namespace Erocten.Foundation.EntityFrameworkCore.DatabaseProvider;

public class SqlServerDatabaseProvider : IDatabaseProvider
{
    public string ProviderName => nameof(SqlServerDatabaseProvider);
}