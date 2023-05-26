namespace Erocten.Foundation.EntityFrameworkCore.DatabaseProvider;

public class SqlServerDatabase : ICurrentDatabase
{
    public string DatabaseName => nameof(SqlServerDatabase);
}