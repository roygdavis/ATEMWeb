using BMDSwitcherAPI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Notifiers
{
    public class LoggerSwitcherNotifer : ISwitcherNotifier
    {
        private readonly ILogger<ISwitcherNotifier> _logger;

        public LoggerSwitcherNotifer(ILogger<ISwitcherNotifier> logger)
        {
            _logger = logger;
        }

        public async Task SwitcherConnected(string productName, string address)
        {
            _logger.LogInformation("Connected to {0} at address {1}", productName, address);
            await Task.CompletedTask;
        }

        public void Notify(_BMDSwitcherEventType eventType, _BMDSwitcherVideoMode coreVideoMode)
        {
            _logger.LogInformation("Switcher event {0}\tVideoMode={1}", eventType.ToString(), coreVideoMode.ToString());
        }

        public void Notify(_BMDSwitcherMixEffectBlockEventType eventType)
        {
            _logger.LogInformation("Mix Effects event {0}", eventType.ToString());
        }
        
        public void Notify(_BMDSwitcherKeyEventType eventType)
        {
            _logger.LogInformation("Keyer event {0}", eventType.ToString());
        }

        public void Notify(_BMDSwitcherDownstreamKeyEventType eventType)
        {
            _logger.LogInformation("Downstream keyer event {0}", eventType.ToString());
        }
    }
}