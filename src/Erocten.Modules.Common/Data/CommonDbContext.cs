using Erocten.Foundation.EntityFrameworkCore;
using Erocten.Modules.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Erocten.Modules.Common.Data;

public class CommonDbContext : BaseDbContext
{
    public CommonDbContext(DbContextOptions<CommonDbContext> options) : base(options)
    {
    }

    public DbSet<Demo> Demos { get; set; } = default!;

}
