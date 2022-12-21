using Erocten.Foundation.EntityFrameworkCore.Conventions;
using Microsoft.EntityFrameworkCore;

namespace Erocten.Foundation.EntityFrameworkCore;

public class BaseDbContext : DbContext
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new DiscriminatorLengthConvention());
    }
}
