using ATEM.Services.Interfaces;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;

namespace ATEM.Services.Hosts
{
    public interface ISwitcherHost
    {
        IBMDSwitcher Connect(string address);
    }
}