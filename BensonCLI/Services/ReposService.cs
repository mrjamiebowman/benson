using System;
using System.Collections.Generic;
using System.Text;
using BensonCLI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BensonCLI.Services
{
    public interface IReposService
    {
        void Run();
    }

    public class ReposService : IReposService
    {
        private readonly ILogger<ReposService> _logger;
        private readonly AppSettings _config;

        public ReposService(ILogger<ReposService> logger, IOptions<AppSettings> config)
        {
            _logger = logger;
            _config = config.Value;
        }

        public void Run()
        {
            _logger.LogWarning($"Wow! We are now in the test service of: {_config.ConsoleTitle}");
        }
    }
}
