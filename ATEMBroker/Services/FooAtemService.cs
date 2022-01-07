using ATEM.Services.Interfaces;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATEM.Services.Services
{
    public class FooAtemService //: IAtemService
    {
        public List<IMixEffectBlock> MixEffectsBlocks { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AtemIPAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public _BMDSwitcherVideoMode VideoMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ProductName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public _BMDSwitcher3GSDIOutputLevel SDI3GOutputLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public _BMDSwitcherPowerStatus PowerStatus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<MixEffectBlockConnectedEventArgs> MixEffectBlockConnectedEvent;
        public event EventHandler<EventArgs> Connected;
        public event EventHandler<EventArgs> DisconnectedEvent;
        public event EventHandler<EventArgs> VideoModeChangedEvent;
        public event EventHandler<EventArgs> MethodForDownConvertedSDChangedEvent;
        public event EventHandler<EventArgs> DownConvertedHDVideoModeChangedEvent;
        public event EventHandler<EventArgs> MultiViewVideoModeChangedEvent;
        public event EventHandler<EventArgs> PowerStatusChangedEvent;
        public event EventHandler<EventArgs> SDI3GOutputLevelChangedEvent;
        public event EventHandler<EventArgs> TypeAutoVideoModeChanged;
        public event EventHandler<EventArgs> AutoVideoModeDetectedChanged;
        public event EventHandler<EventArgs> SuperSourceCascadeChanged;
        public event EventHandler<EventArgs> TimeCodeChanged;
        public event EventHandler<EventArgs> TimeCodeLockedChanged;
        public event EventHandler<EventArgs> TimeCodeModeChanged;

        public void Connect(string address)
        {
            throw new NotImplementedException();
        }
    }
}
