using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Notifiers
{
    public interface ISwitcherNotifier : IBMDSwitcherCallback, IBMDSwitcherMixEffectBlockCallback, IBMDSwitcherKeyCallback, IBMDSwitcherDownstreamKeyCallback
    {
        Task SwitcherConnected(string productName, string address);
    }
}