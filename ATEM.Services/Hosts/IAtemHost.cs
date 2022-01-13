using ATEM.Services.Interfaces;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;

namespace ATEM.Services.Hosts
{
    public interface IAtemHost: IDisposable, IBMDSwitcherCallback
    {
        string AtemIPAddress { get; set; }
        List<IMixEffectBlock> MixEffectsBlocks { get; set; }
        _BMDSwitcherPowerStatus PowerStatus { get; set; }
        string ProductName { get; set; }
        _BMDSwitcher3GSDIOutputLevel SDI3GOutputLevel { get; set; }
        _BMDSwitcherVideoMode VideoMode { get; set; }

        event EventHandler<EventArgs> AutoVideoModeDetectedChanged;
        event EventHandler<EventArgs> Connected;
        event EventHandler<EventArgs> DisconnectedEvent;
        event EventHandler<EventArgs> DownConvertedHDVideoModeChangedEvent;
        event EventHandler<EventArgs> MethodForDownConvertedSDChangedEvent;
        event EventHandler<MixEffectBlockConnectedEventArgs> MixEffectBlockConnectedEvent;
        event EventHandler<EventArgs> MultiViewVideoModeChangedEvent;
        event EventHandler<EventArgs> PowerStatusChangedEvent;
        event EventHandler<EventArgs> SDI3GOutputLevelChangedEvent;
        event EventHandler<EventArgs> SuperSourceCascadeChanged;
        event EventHandler<EventArgs> TimeCodeChanged;
        event EventHandler<EventArgs> TimeCodeLockedChanged;
        event EventHandler<EventArgs> TimeCodeModeChanged;
        event EventHandler<EventArgs> TypeAutoVideoModeChanged;
        event EventHandler<EventArgs> VideoModeChangedEvent;

        void Connect(string address);
        void Dispose();
        void Notify(_BMDSwitcherEventType eventType, _BMDSwitcherVideoMode coreVideoMode);
        bool HasMixEffectBlocks();
    }
}