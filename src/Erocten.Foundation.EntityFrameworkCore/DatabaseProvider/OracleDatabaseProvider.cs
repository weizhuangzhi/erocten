namespace Erocten.Foundation.EntityFrameworkCore;

public class OracleDatabaseProvider : IDatabaseProvider
{
    public string ProviderName => nameof(OracleDatabaseProvider);
}