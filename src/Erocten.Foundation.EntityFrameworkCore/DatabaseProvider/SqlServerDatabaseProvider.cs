namespace Erocten.Foundation.EntityFrameworkCore;

public class SqlServerDatabaseProvider : IDatabaseProvider
{
    public string ProviderName => nameof(SqlServerDatabaseProvider);
}