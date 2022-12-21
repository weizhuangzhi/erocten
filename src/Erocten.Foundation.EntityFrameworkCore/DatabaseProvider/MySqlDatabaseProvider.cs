namespace Erocten.Foundation.EntityFrameworkCore;

public class MySqlDatabaseProvider : IDatabaseProvider
{
    public string ProviderName => nameof(MySqlDatabaseProvider);
}