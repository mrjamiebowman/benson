using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BensonCLI.Enums;
using BensonCLI.Helpers;
using BensonCLI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BensonCLI.Services
{
    public interface IVpnService
    {
        void Run();
    }

    public class VpnService : IVpnService
    {
        private readonly ILogger<VpnService> _logger;
        private readonly AppSettings _config;

        public VpnService(ILogger<VpnService> logger, IOptions<AppSettings> config)
        {
            _logger = logger;
            _config = config.Value;
        }

        public bool VpnConnected = false;
        public string IP = String.Empty;
        private string InterfaceName = String.Empty;
        private string VpnMessage = String.Empty;

        public async void Run()
        {
            // initial run
            await CheckVpnAsync();
            await DisplayStatusAsync();

            // monitor
            //Task taskVpn = new Task(async () =>
            //{

            //});

            Thread t = new Thread(() =>
            {
                while (true)
                {
                    var prevVpnConnected = VpnConnected;
                    Thread.Sleep(1000);
                    _ = CheckVpnAsync();

                    if (prevVpnConnected != VpnConnected)
                    {
                        _ = DisplayStatusAsync();
                    }
                }
            });

            t.Start();
        }

        private async Task CheckVpnAsync()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface intf in interfaces)
                {
                    if (intf.OperationalStatus == OperationalStatus.Up)
                    {
                        // Cisco AnyConnect Secure Mobility Client Virtual Miniport Adapter for Windows x64
                        if ((intf.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                             intf.Description.Contains("Cisco AnyConnect")) &&
                            (intf.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                        {
                            InterfaceName = intf.Description;
                            IPv4InterfaceStatistics statistics = intf.GetIPv4Statistics();
                            IP = intf.GetIPProperties().UnicastAddresses.First(x =>
                                    x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Address
                                .ToString();
                            VpnConnected = true;
                            break;
                        }
                        else
                        {
                            VpnConnected = false;
                        }
                    }
                }
            }
        }

        private async Task DisplayStatusAsync()
        {
            var status = ConsoleStatusType.Warning;
            string message = VpnConnected == true
                ? $"Connected to {InterfaceName.Substring(0, 39)}..."
                : "Disconnected";
            if (VpnConnected == true)
            {
                status = ConsoleStatusType.Positive;
            }

            //_logger.LogWarning($"VPN Status: {message}");
            ConsoleOutput.WriteLine($"VPN Status: {message}", status);

            if (VpnConnected == true)
                ConsoleOutput.WriteLine($"IP: {IP}", ConsoleStatusType.Positive);
        }
    }
}
