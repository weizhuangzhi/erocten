using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Erocten.Foundation.EntityFrameworkCore.Conventions;

public class TableNameConvention : IModelFinalizingConvention
{
    private const string DefaultNameSeparator = "_";
    private const string DefaultNamePrefix = "ET";

    public TableNameConvention()
    {
    }

    public TableNameConvention(string prefix, string separator, bool useModuleName, bool useClrTypeName)
    {
        Prefix = prefix;
        Separator = separator;
        UseModuleName = useModuleName;
        UseClrTypeName = useClrTypeName;
    }

    public static string Prefix { get; set; } = DefaultNamePrefix;

    public static string Separator { get; set; } = DefaultNameSeparator;

    public bool UseModuleName { get; set; } = true;

    public bool UseClrTypeName { get; set; } = true;

    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            var tableName = UseClrTypeName ? entityType.ClrType.Name : entityType.GetTableName();

            var moduleName = string.Empty;
            if (UseModuleName)
            {
                var entityTypeNameParts = entityType.Name.Split('.');
                moduleName = entityTypeNameParts.Length > 2 ? entityTypeNameParts[2] : entityType.ClrType.Module.Name.Split('.')[^2];
                moduleName = $"{moduleName}{Separator}";
            }

            entityType.Builder.ToTable($"{Prefix}{Separator}{moduleName}{tableName}");
        }
    }
}