namespace Erocten.Foundation.EntityFrameworkCore.DatabaseProvider;

public class MySqlDatabase : ICurrentDatabase
{
    public string DatabaseName => nameof(MySqlDatabase);
}