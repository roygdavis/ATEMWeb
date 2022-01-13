using ATEM.Services.Hosts;
using ATEM.Services.Interfaces;
using BMDSwitcherAPI;
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

        public AtemService(IAtemHost atemHost)
        {
            _atemHost = atemHost;
        }

        public void Connect(string address)
        {
            if (_atemHost is null)
                throw new ArgumentNullException("Atem Host is null");

            _atemHost.Connect(address);
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

        public Task SetPgmInput(int meBlockIndex, long inputId)
        {
            var meBlock = GetMeBlockByIndex(meBlockIndex);
            meBlock.ProgramInput = inputId;

            return Task.CompletedTask;
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