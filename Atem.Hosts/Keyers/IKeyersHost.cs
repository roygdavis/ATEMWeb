using Atem.Hosts.MixEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Keyers
{
    public interface IKeyersHost
    {
        IMixEffectsHost MixEffectsHost { get; set; }
    }
}
