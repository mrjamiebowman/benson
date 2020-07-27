using System;
using System.IO;
using BensonCLI.Enums;
using BensonCLI.Helpers;
using BensonCLI.Models;
using BensonCLI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BensonCLI
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"
 /$$$$$$$                                                           /$$$$$$  /$$       /$$$$$$
| $$__  $$                                                         /$$__  $$| $$      |_  $$_/
| $$  \ $$  /$$$$$$  /$$$$$$$   /$$$$$$$  /$$$$$$  /$$$$$$$       | $$  \__/| $$        | $$  
| $$$$$$$  /$$__  $$| $$__  $$ /$$_____/ /$$__  $$| $$__  $$      | $$      | $$        | $$  
| $$__  $$| $$$$$$$$| $$  \ $$|  $$$$$$ | $$  \ $$| $$  \ $$      | $$      | $$        | $$  
| $$  \ $$| $$_____/| $$  | $$ \____  $$| $$  | $$| $$  | $$      | $$    $$| $$        | $$  
| $$$$$$$/|  $$$$$$$| $$  | $$ /$$$$$$$/|  $$$$$$/| $$  | $$      |  $$$$$$/| $$$$$$$$ /$$$$$$
|_______/  \_______/|__/  |__/|_______/  \______/ |__/  |__/       \______/ |________/|______/
                                                                                              
   @mrjamiebowman - 2020
");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Benson Command Line Tool");
            Console.WriteLine("");

            ConsoleOutput.WriteLine($"Loading Benson CLI...", ConsoleStatusType.Positive);

            // display tools

            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // run app
            await serviceProvider.GetService<App>().Run(args);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add logging
            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                //loggingBuilder.AddSerilog();
                loggingBuilder.AddDebug();
            });

            // build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(x => configuration.GetSection("Configuration").Bind(x));
            ConfigureConsole(configuration);

            // add app services
            serviceCollection.AddTransient<IVpnService, VpnService>();
            serviceCollection.AddTransient<IReposService, ReposService>();

            // add app
            serviceCollection.AddTransient<App>();
        }

        private static void ConfigureConsole(IConfigurationRoot configuration)
        {
            System.Console.Title = configuration.GetSection("Configuration:ConsoleTitle").Value;
        }
    }
}
