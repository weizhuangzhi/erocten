namespace Erocten.Foundation.EntityFrameworkCore.DatabaseProvider;

public class PostgreSqlDatabase : ICurrentDatabase
{
    public string DatabaseName => nameof(PostgreSqlDatabase);
}