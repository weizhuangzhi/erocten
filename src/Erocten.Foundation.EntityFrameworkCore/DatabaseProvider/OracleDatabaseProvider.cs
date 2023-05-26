namespace Erocten.Foundation.EntityFrameworkCore.DatabaseProvider;

public class OracleDatabaseProvider : IDatabaseTypeProvider
{
    public string ProviderName => nameof(OracleDatabaseProvider);
}