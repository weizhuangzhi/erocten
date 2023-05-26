namespace Erocten.Foundation.EntityFrameworkCore.Migrations
{
    public interface IMigrateData
    {
        void OnBeforeMigrate(string migrationClassName);
        void OnAfterMigrate(string migrationClassName);
    }
}
