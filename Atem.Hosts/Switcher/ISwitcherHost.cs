using Atem.Hosts.Core;
using Atem.Hosts.MixEffects;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Switcher
{
    public interface ISwitcherHost : IDisposable, ISwitcherMethods
    {
        Dictionary<int, IMixEffectsHost> MixEffects { get; set; }
    }
}