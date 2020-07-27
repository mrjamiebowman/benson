using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;
using System.Threading.Tasks;
using BensonCLI.Models;
using BensonCLI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BensonCLI
{
    public class App
    {
        private readonly IVpnService _vpnService;
        private readonly ILogger<App> _logger;
        private readonly AppSettings _config;

        public List<string> Apps = new List<string>()
        {
            "repos",
            "appcheck"
        };

        public App(
            IVpnService vpnService,
            IOptions<AppSettings> config,
            ILogger<App> logger)
        {
            _vpnService = vpnService;
            _logger = logger;
            _config = config.Value;
        }

        public async Task Run(string[] args)
        {
            _logger.LogInformation($"This is a console application for {_config.ConsoleTitle}");
            _vpnService.Run();

            if (args != null)
            {
                // build commands
                foreach (string arg in args)
                {
                    if (IsCommandArg(arg))
                    {

                    }
                }

                // execute commands
                await BuildCommands(args);
            }
            else
            {
                // display main menu
                MainMenu();
            }
        }

        private static async Task<int> BuildCommands(string[] args)
        {
            // repos
            var reposCommand = new Command("repos", "Repository mapper.");

            var repoOptApp = new Option(aliases: new string[] { "--app", "-a" }, "Application name.");
            reposCommand.AddOption(repoOptApp);

            var repoOptCategory = new Option(aliases: new string[] { "--category", "-c" }, "Category: app, db, etl.");
            reposCommand.AddOption(repoOptCategory);

            var rootCommand = new RootCommand
            {
                reposCommand,
                new Command("apptest", "Automated application testing."),
            };

            return await rootCommand.InvokeAsync(args);
        }

        private bool IsCommandArg(string arg)
        {
            arg = arg.ToLower();

            if (Apps.Contains(arg))
            {
                return true;
            }

            return false;
        }

        private void MainMenu()
        {
            Console.WriteLine("");
            Console.WriteLine(" Please choose a Bensom CLI App:");
            Console.WriteLine(" (R)epos - Repository Mapper");
            Console.WriteLine(" (H)elp, E(x)it");
            Console.Write("\r\nSelect an option: ");

        Startover:

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.R:
                    break;
                case ConsoleKey.A:
                    break;
                case ConsoleKey.H:
                    break;
                case ConsoleKey.X:
                    Console.WriteLine("\r\nKTHXBYE!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please enter a character for the app you want to use.");
                    goto Startover;
            }
        }
    }
}
