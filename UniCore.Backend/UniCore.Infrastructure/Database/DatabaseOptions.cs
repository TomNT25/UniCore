namespace UniCore.Infrastructure.Database
{
    public class DatabaseOptions
    {
        public string Provider { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public bool AutoMigrate { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
        public int CommandTimeout { get; set; }
        public int MaxRetryCount { get; set; }
        public int MaxRetryDelay { get; set; }
        public bool EnableQuerySplitting { get; set; }
        public int PoolSize { get; set; }
        public bool EnableHealthChecks { get; set; }
        public int HealthCheckInterval { get; set; }
    }
}
