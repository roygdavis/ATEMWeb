using Atem.Hosts.Legacy.Interfaces;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;

namespace Atem.Hosts.Legacy.Hosts
{
    public interface ISwitcherHost
    {
        IBMDSwitcher Connect(string address);
    }
}