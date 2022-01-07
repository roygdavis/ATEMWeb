using ATEM.Services;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATEM.Services.Interfaces
{
    public interface IAtemService 
    { 
        List<IMixEffectBlock> MixEffectsBlocks { get; set; }
        string AtemIPAddress { get; set; }
        _BMDSwitcherVideoMode VideoMode { get; set; }
        string ProductName { get; set; }
        _BMDSwitcher3GSDIOutputLevel SDI3GOutputLevel { get; set; }
        _BMDSwitcherPowerStatus PowerStatus { get; set; }


        /// <summary>
        /// Fired when a new Mix Effects block as detected and added
        /// </summary>
        event EventHandler<MixEffectBlockConnectedEventArgs> MixEffectBlockConnectedEvent;

        /// <summary>
        /// Called when the Atem is connected successfully
        /// </summary>
        event EventHandler<EventArgs> Connected;

        /// <summary>
        /// Called when the Atem is disconnected successfully
        /// </summary>
        event EventHandler<EventArgs> DisconnectedEvent;
        event EventHandler<EventArgs> VideoModeChangedEvent;
        event EventHandler<EventArgs> MethodForDownConvertedSDChangedEvent;
        event EventHandler<EventArgs> DownConvertedHDVideoModeChangedEvent;
        event EventHandler<EventArgs> MultiViewVideoModeChangedEvent;
        event EventHandler<EventArgs> PowerStatusChangedEvent;
        event EventHandler<EventArgs> SDI3GOutputLevelChangedEvent;
        event EventHandler<EventArgs> TypeAutoVideoModeChanged;
        event EventHandler<EventArgs> AutoVideoModeDetectedChanged;
        event EventHandler<EventArgs> SuperSourceCascadeChanged;
        event EventHandler<EventArgs> TimeCodeChanged;
        event EventHandler<EventArgs> TimeCodeLockedChanged;
        event EventHandler<EventArgs> TimeCodeModeChanged;

        void Connect(string address);
    }
}