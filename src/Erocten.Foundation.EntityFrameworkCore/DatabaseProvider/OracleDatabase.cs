namespace Erocten.Foundation.EntityFrameworkCore.DatabaseProvider;

public class OracleDatabase : ICurrentDatabase
{
    public string DatabaseName => nameof(OracleDatabase);
}