namespace Erocten.Foundation.EntityFrameworkCore.DatabaseProvider;

public class SqlServerDatabaseProvider : IDatabaseTypeProvider
{
    public string ProviderName => nameof(SqlServerDatabaseProvider);
}