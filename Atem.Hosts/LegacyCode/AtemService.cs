using Atem.Hosts.Legacy.Hosts;
using Atem.Hosts.Legacy.Hosts.MixEffects;
using Atem.Hosts.Legacy.Hubs;
using Atem.Hosts.Legacy.Interfaces;
using BMDSwitcherAPI;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Legacy.Services
{
    public class AtemService : IAtemService
    {
        private readonly ISwitcherHost _switcherHost;
        private IMixEffectsHost _mixEffectsHost;
        private readonly IHubContext<ATEMEventsHub> _hub;
        private readonly AtemServicesConfiguration _config;

        public AtemService(ISwitcherHost atemHost, IHubContext<ATEMEventsHub> hub, AtemServicesConfiguration configuration)
        {
            _switcherHost = atemHost;
            _hub = hub;
            _config = configuration;
        }

        public async Task Connect(string address)
        {
            if (_switcherHost is null)
                throw new ArgumentNullException("Atem Host is null");

            var switcher = _switcherHost.Connect(address);
            if (_config.EnableEvents)
                await _hub.Clients.All.SendAsync("connected");

            _mixEffectsHost = new MixEffectsHost(switcher);
            _mixEffectsHost.DiscoverMixEffects();
        }

        public Task<IMixEffectBlock> GetMeBlock(int meId)
        {
            if (_mixEffectsHost.HasMixEffectBlocks)
            {
                return Task.FromResult(_mixEffectsHost.MixEffectsBlocks[meId]);
            }
            else
                throw new MixEffectsNullException();
        }

        public Task<IEnumerable<IMixEffectBlock>> GetMeBlocks()
        {
            if (_mixEffectsHost.HasMixEffectBlocks)
            {
                return Task.FromResult(_mixEffectsHost.MixEffectsBlocks.AsEnumerable());
            }
            else
                throw new MixEffectsNullException();
        }

        public async Task SetPgmInput(int meBlockIndex, long inputId)
        {
            var meBlock = GetMeBlockByIndex(meBlockIndex);
            meBlock.ProgramInput = inputId;

            if (_config.EnableEvents)
                await _hub.Clients.All.SendAsync("updated", inputId);
        }

        public Task SetPvwInput(int meBlockIndex, long inputId)
        {
            var meBlock = GetMeBlockByIndex(meBlockIndex);
            meBlock.PreviewInput = inputId;

            return Task.CompletedTask;
        }

        private IMixEffectBlock GetMeBlockByIndex(int meBlockIndex)
        {
            if (!_mixEffectsHost.HasMixEffectBlocks)
                throw new MixEffectsNullException();

            return _mixEffectsHost.MixEffectsBlocks[meBlockIndex];
        }

        private Task SetKey(int meBlockIndex, int keyerId)
        {
            if (!_mixEffectsHost.HasMixEffectBlocks)
                throw new MixEffectsNullException();

            //_mixEffectsHost.MixEffectsBlocks[meBlockIndex].
            return Task.CompletedTask;
        }
    }
}