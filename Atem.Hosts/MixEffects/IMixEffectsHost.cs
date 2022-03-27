using Atem.Hosts.Keyers;
using Atem.Hosts.Switcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.MixEffects
{
    public interface IMixEffectsHost : IDisposable
    {
        ISwitcherHost SwitcherHost { get; set; }
        Dictionary<int, IKeyersHost> KeyersHosts { get; set; }

        Task DiscoverKeyers();
        Task SetPGM(long input);
        Task SetPVW(long input);
    }
}