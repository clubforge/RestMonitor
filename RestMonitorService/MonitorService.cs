using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RestMonitorService
{
    public class MonitorService : BackgroundService
    {
        private readonly ILogger<MonitorService> _logger;
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        public MonitorService(ILogger<MonitorService> logger, HttpClient client, IConfiguration config)
        {
            _logger = logger;
            _client = client;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _logger.LogInformation("Checking remote service at: {Time}", DateTimeOffset.Now);
                // Set ApiHealthUrl in appsettings.json
                var request = await _client.GetAsync(_config["ApiHealthUrl"], token);
                if (!request.IsSuccessStatusCode)
                {
                    _logger.LogError("Unable to read from api service: [{Code}] {Message}",
                        request.StatusCode,
                        request.ReasonPhrase);
                }
                else
                {
                    _logger.LogInformation("API ({Url}) is responding successfully", _config["ApiHealthUrl"]);
                }

                await Task.Delay(1000, token);
            }

            _client.Dispose();
        }
    }
}