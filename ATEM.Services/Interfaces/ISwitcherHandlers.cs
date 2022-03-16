using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATEM.Services.Interfaces
{
    internal interface ISwitcherHandlers<T>
    {
        _BMDSwitcherVideoMode VideoMode { get; set; }
        _BMDSwitcherDownConversionMethod MethodForDownConvertedSD { get; set; }
        _BMDSwitcherDownConversionMethod DownConvertedHDVideoMode { get; set; }
        _BMDSwitcherVideoMode MultiViewVideoMode { get; set; }
        _BMDSwitcherPowerStatus PowerStatus { get; set; }
        bool Disconnected { get; set; }
        _BMDSwitcher3GSDIOutputLevel SDI3GOutputLevel { get; set; }
        long TimeCode { get; set; }
        bool TimeCodeLocked { get; set; }
        _BMDSwitcherTimeCodeMode TimeCodeMode { get; set; }
        bool SuperSourceCascade { get; set; }
        bool AutoVideoMode { get; set; }
        bool AutoVideoModeDetected { get; set; }
        string AtemIPAddress { get; set; }
        string ProductName { get; set; }

        
    }
}