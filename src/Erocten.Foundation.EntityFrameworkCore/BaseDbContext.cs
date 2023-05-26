using Erocten.Foundation.EntityFrameworkCore.Conventions;
using Erocten.Foundation.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Erocten.Foundation.EntityFrameworkCore;

public class BaseDbContext : DbContext
{
    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ReplaceService<IMigrator, PowerMigrator>();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new MaxStringLengthConvention());
        configurationBuilder.Conventions.Add(_ => new TableNameConvention());
    }
}
