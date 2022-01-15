using ATEM.Services.Interfaces;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;

namespace ATEM.Services.Hosts
{
    public interface ISwitcherHost
    {
        string AtemIPAddress { get; set; }
        string ProductName { get; set; }
        bool IsConnected { get; set; }
        _BMDSwitcherPowerStatus PowerStatus { get; set; }
        _BMDSwitcher3GSDIOutputLevel SDI3GOutputLevel { get; set; }
        _BMDSwitcherVideoMode VideoMode { get; set; }

        IBMDSwitcher Connect(string address);
    }
}