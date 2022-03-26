using Atem.Hosts.Legacy.Interfaces;
using System;
using System.Collections.Generic;

namespace Atem.Hosts.Legacy.Hosts.MixEffects
{
    public interface IMixEffectsHost
    {
        List<IMixEffectBlock> MixEffectsBlocks { get; set; }
        bool HasMixEffectBlocks { get; }

        void DiscoverMixEffects();
    }
}