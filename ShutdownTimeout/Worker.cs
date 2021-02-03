using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ShutdownTimeout
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOptions<HostOptions> _options;

        public Worker(ILogger<Worker> logger, IOptions<HostOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Shutdown timeout: {0}", _options.Value.ShutdownTimeout);
            _logger.LogInformation("DOTNET_SHUTDOWNTIMEOUTSECONDS: " + Environment.GetEnvironmentVariable("DOTNET_SHUTDOWNTIMEOUTSECONDS"));
            _logger.LogInformation("ASPNETCORE_SHUTDOWNTIMEOUTSECONDS " + Environment.GetEnvironmentVariable("ASPNETCORE_SHUTDOWNTIMEOUTSECONDS"));
            _logger.LogInformation("SHUTDOWNTIMEOUTSECONDS: " + Environment.GetEnvironmentVariable("SHUTDOWNTIMEOUTSECONDS"));
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000);
            }
            
            _logger.LogInformation("Shutdown has been requested");
            for (int i = 15; i > 0; i--)
            {
                _logger.LogInformation($"Shutting down in {i}");
                await Task.Delay(1000);
            }
            
            _logger.LogInformation("Shutdown completed");
        }
    }
}