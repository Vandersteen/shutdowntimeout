using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ShutdownTimeout
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                await CreateHostBuilder(args).Build().RunAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }

            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    
                    //If you uncomment this, the worker will shutdown 'cleanly'
                    //using the env variables does not seem to affect this setting
                    //see launchSettings.json
                    // services.Configure<HostOptions>(option =>
                    // {
                    //     option.ShutdownTimeout = System.TimeSpan.FromSeconds(20);
                    // });
                    
                });
    }
}