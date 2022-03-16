using Atem.Hosts.MixEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Switcher
{
    public interface ISwitcherHost
    {
        Dictionary<int, IMixEffectsHost> MixEffects { get; set; }
        
        Task Connect(string address);
    }
}
