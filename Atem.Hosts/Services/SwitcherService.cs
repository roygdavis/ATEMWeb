using Atem.Hosts.Switcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Services
{
    public class SwitcherService : ISwitcherService
    {
        private readonly ISwitcherHost _switcherHost;

        public SwitcherService(ISwitcherHost switcherHost)
        {
            _switcherHost = switcherHost;
        }

        public async Task Connect(string address)
        {
            await _switcherHost.Connect(address);
        }

        public async Task Disconnect()
        {
            await Task.CompletedTask;
        }

        public async Task SetPGM(int switcherIndex, long input)
        {
            await _switcherHost.MixEffects[switcherIndex].SetPGM(input);
        }

        public async Task SetPVW(int switcherIndex, long input)
        {
            await _switcherHost.MixEffects[switcherIndex].SetPVW(input);
        }
    }
}