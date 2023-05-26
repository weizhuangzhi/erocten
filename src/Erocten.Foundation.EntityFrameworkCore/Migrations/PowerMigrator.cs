using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Erocten.Foundation.EntityFrameworkCore.Migrations
{
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
    public class PowerMigrator : Migrator
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IRelationalDatabaseCreator _databaseCreator;
        private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;
        private readonly IMigrationCommandExecutor _migrationCommandExecutor;
        private readonly IRelationalConnection _connection;
        private readonly ICurrentDbContext _currentContext;
        private readonly IDiagnosticsLogger<DbLoggerCategory.Migrations> _logger;
        private readonly IRelationalCommandDiagnosticsLogger _commandLogger;

        public PowerMigrator(IMigrationsAssembly migrationsAssembly,
            IHistoryRepository historyRepository,
            IDatabaseCreator databaseCreator,
            IMigrationsSqlGenerator migrationsSqlGenerator,
            IRawSqlCommandBuilder rawSqlCommandBuilder,
            IMigrationCommandExecutor migrationCommandExecutor,
            IRelationalConnection connection,
            ISqlGenerationHelper sqlGenerationHelper,
            ICurrentDbContext currentContext,
            IModelRuntimeInitializer modelRuntimeInitializer,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger,
            IRelationalCommandDiagnosticsLogger commandLogger,
            IDatabaseProvider databaseProvider)
            : base(migrationsAssembly,
                  historyRepository,
                  databaseCreator,
                  migrationsSqlGenerator,
                  rawSqlCommandBuilder,
                  migrationCommandExecutor,
                  connection,
                  sqlGenerationHelper,
                  currentContext,
                  modelRuntimeInitializer,
                  logger,
                  commandLogger,
                  databaseProvider)
        {
            _historyRepository = historyRepository;
            _databaseCreator = (IRelationalDatabaseCreator)databaseCreator;
            _rawSqlCommandBuilder = rawSqlCommandBuilder;
            _migrationCommandExecutor = migrationCommandExecutor;
            _connection = connection;
            _currentContext = currentContext;
            _logger = logger;
            _commandLogger = commandLogger;
        }

        public override void Migrate(string? targetMigration = null)
        {
            _logger.MigrateUsingConnection(this, _connection);

            if (!_historyRepository.Exists())
            {
                if (!_databaseCreator.Exists())
                {
                    _databaseCreator.Create();
                }

                var command = _rawSqlCommandBuilder.Build(
                    _historyRepository.GetCreateScript());

                command.ExecuteNonQuery(
                    new RelationalCommandParameterObject(
                        _connection,
                        null,
                        null,
                        _currentContext.Context,
                        _commandLogger, CommandSource.Migrations));
            }

            var commandLists = GetMigrationCommandLists(_historyRepository.GetAppliedMigrations(), targetMigration);
            foreach (var commandList in commandLists)
            {
                if (_currentContext.Context is IMigrateData beforeContext)
                {
                    beforeContext.OnBeforeMigrate(commandList.Migration.GetType().Name);
                }

                _migrationCommandExecutor.ExecuteNonQuery(commandList.CommandList(), _connection);

                if (_currentContext.Context is IMigrateData afterContext)
                {
                    afterContext.OnAfterMigrate(commandList.Migration.GetType().Name);
                }
            }
        }
        private IEnumerable<(Func<IReadOnlyList<MigrationCommand>> CommandList, Migration Migration)> GetMigrationCommandLists(
            IReadOnlyList<HistoryRow> appliedMigrationEntries,
            string? targetMigration = null)
        {
            PopulateMigrations(
                appliedMigrationEntries.Select(t => t.MigrationId),
                targetMigration,
                out var migrationsToApply,
                out var migrationsToRevert,
                out var actualTargetMigration);

            for (var i = 0; i < migrationsToRevert.Count; i++)
            {
                var migration = migrationsToRevert[i];

                var index = i;
                yield return (() =>
                {
                    _logger.MigrationReverting(this, migration);
                    return GenerateDownSql(
                        migration,
                        index != migrationsToRevert.Count - 1
                            ? migrationsToRevert[index + 1]
                            : actualTargetMigration);
                }, migration);
            }

            foreach (var migration in migrationsToApply)
            {
                yield return (() =>
                {
                    _logger.MigrationApplying(this, migration);
                    return GenerateUpSql(migration);
                }, migration);
            }

            if (migrationsToRevert.Count + migrationsToApply.Count == 0)
            {
                _logger.MigrationsNotApplied(this);
            }
        }
    }
}
