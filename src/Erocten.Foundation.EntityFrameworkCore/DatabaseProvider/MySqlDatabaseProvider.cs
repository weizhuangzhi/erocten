namespace Erocten.Foundation.EntityFrameworkCore.DatabaseProvider;

public class MySqlDatabaseProvider : IDatabaseProvider
{
    public string ProviderName => nameof(MySqlDatabaseProvider);
}