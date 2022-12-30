using Erocten.Foundation.EntityFrameworkCore.Conventions;
using Microsoft.EntityFrameworkCore;

namespace Erocten.Foundation.EntityFrameworkCore;

public class BaseDbContext : DbContext
{
    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new MaxStringLengthConvention());
        configurationBuilder.Conventions.Add(_ => new TableNameConvention());
    }
}
