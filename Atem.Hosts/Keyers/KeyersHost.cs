using Atem.Hosts.MixEffects;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Keyers
{
    public class KeyersHost : IKeyersHost
    {
        private readonly IBMDSwitcherKey _keyer;
        public IMixEffectsHost MixEffectsHost { get; set; }

        public KeyersHost(IMixEffectsHost mixEffectsHost, IBMDSwitcherKey keyer)
        {
            _keyer = keyer;
            MixEffectsHost = mixEffectsHost;
        }
    }
}