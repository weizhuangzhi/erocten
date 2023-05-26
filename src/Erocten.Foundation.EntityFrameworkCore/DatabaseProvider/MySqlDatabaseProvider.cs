namespace Erocten.Foundation.EntityFrameworkCore.DatabaseProvider;

public class MySqlDatabaseProvider : IDatabaseTypeProvider
{
    public string ProviderName => nameof(MySqlDatabaseProvider);
}