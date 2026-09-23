using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace UniCore.Infrastructure.Database.Interceptors
{
    public class LoggingDbConnectionInterceptor : DbConnectionInterceptor
    {
        private readonly ILogger<LoggingDbConnectionInterceptor> _logger;

        public LoggingDbConnectionInterceptor(ILogger<LoggingDbConnectionInterceptor> logger)
        {
            _logger = logger;
        }

        public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        {
            _logger.LogInformation("Database connection opened. Duration: {Duration}ms", eventData.Duration.TotalMilliseconds);
            base.ConnectionOpened(connection, eventData);
        }

        public override Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Database connection opened async. Duration: {Duration}ms", eventData.Duration.TotalMilliseconds);
            return base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
        }

        public override void ConnectionClosed(DbConnection connection, ConnectionEndEventData eventData)
        {
            _logger.LogInformation("Database connection closed. Duration: {Duration}ms", eventData.Duration.TotalMilliseconds);
            base.ConnectionClosed(connection, eventData);
        }
    }
}
