using ATEM.Services.Hosts;
using ATEM.Services.Hubs;
using ATEM.Services.Interfaces;
using BMDSwitcherAPI;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATEM.Services.Services
{
    public class AtemService : IAtemService
    {
        private readonly IAtemHost _atemHost;
        private readonly IHubContext<ATEMEventsHub> _hub;
        private readonly AtemServicesConfiguration _config;

        public AtemService(IAtemHost atemHost, IHubContext<ATEMEventsHub> hub, AtemServicesConfiguration configuration)
        {
            _atemHost = atemHost;
            _hub = hub;
            _config = configuration;
        }

        public async Task Connect(string address)
        {
            if (_atemHost is null)
                throw new ArgumentNullException("Atem Host is null");

            _atemHost.Connect(address);
            if (_config.EnableEvents)
                await _hub.Clients.All.SendAsync("connected");
        }

        public Task<IMixEffectBlock> GetMeBlock(int meId)
        {
            if (_atemHost.HasMixEffectBlocks())
            {
                return Task.FromResult(_atemHost.MixEffectsBlocks[meId]);
            }
            else
                throw new MixEffectsNullException();
        }

        public Task<IEnumerable<IMixEffectBlock>> GetMeBlocks()
        {
            if (_atemHost.HasMixEffectBlocks())
            {
                return Task.FromResult(_atemHost.MixEffectsBlocks.AsEnumerable());
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
            if (!_atemHost.HasMixEffectBlocks())
                throw new MixEffectsNullException();

            return _atemHost.MixEffectsBlocks[meBlockIndex];
        }
    }
}