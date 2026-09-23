using Microsoft.EntityFrameworkCore;
using UniCore.Infrastructure.Database;

namespace UniCore.Infrastructure.Extension
{
    public static class DatabaseProviderExtensions
    {
        public static DbContextOptionsBuilder ApplyConfiguration(
            this DbContextOptionsBuilder optionsBuilder,
            DatabaseOptions dbOptions)
        {
            return dbOptions.Provider switch
            {
                "SQL Server" => optionsBuilder.UseSqlServer(dbOptions.ConnectionString, sqlServerOptions =>
                                {
                                    sqlServerOptions.CommandTimeout(dbOptions.CommandTimeout);
                                    sqlServerOptions.EnableRetryOnFailure(
                                        maxRetryCount: dbOptions.MaxRetryCount,
                                        maxRetryDelay: TimeSpan.FromSeconds(dbOptions.MaxRetryDelay),
                                        errorNumbersToAdd: null);

                                    if (dbOptions.EnableQuerySplitting)
                                    {
                                        sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                                    }
                                }),
                _ => throw new NotSupportedException($"Database provider '{dbOptions.Provider}' is not supported.")
            };
        }
    }
}