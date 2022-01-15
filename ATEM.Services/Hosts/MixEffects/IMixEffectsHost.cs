using ATEM.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace ATEM.Services.Hosts.MixEffects
{
    public interface IMixEffectsHost
    {
        List<IMixEffectBlock> MixEffectsBlocks { get; set; }
        bool HasMixEffectBlocks { get; }

        void DiscoverMixEffects();
    }
}