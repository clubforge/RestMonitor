using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RestMonitorService
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<HttpClient>(new HttpClient());
                    services.AddHostedService<MonitorService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}