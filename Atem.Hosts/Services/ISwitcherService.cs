using Atem.Hosts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Services
{
    public interface ISwitcherService : ISwitcherMethods, IMixEffectsMethods
    {
        Task Disconnect();
        Task SetMixEffectsContext(int mixEffectIndex);
    }
}