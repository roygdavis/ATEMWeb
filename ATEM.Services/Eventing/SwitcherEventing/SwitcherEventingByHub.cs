using ATEM.Services.Hosts;
using BMDSwitcherAPI;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATEM.Services.Eventing.SwitcherEventing
{
    public class SwitcherEventingByHub : SwitcherEventingByHubBase
    {
        private readonly IHubContext<Hubs.ATEMEventsHub> _hub;
        private readonly ISwitcherHost _host;

        public SwitcherEventingByHub(IHubContext<Hubs.ATEMEventsHub> hub, ISwitcherHost host)
        {
            _hub = hub;
            _host = host;

            if (host is not null)
            {
                //_host.DisconnectedEvent += DisconnectedEvent;
                //_host.AutoVideoModeDetectedChangedEvent += AutoVideoModeDetectedChangedEvent;
                //_host.PowerStatusChangedEvent += PowerStatusChangedEvent;
                //_host.SDI3GOutputLevelChangedEvent += SDI3GOutputLevelChangedEvent;
                //_host.SuperSourceCascadeChangedEvent += SuperSourceCascadeChangedEvent;
                //_host.TimeCodeChangedEvent += TimeCodeChangedEvent;
                //_host.TimeCodeLockedChangedEvent += TimeCodeLockedChangedEvent;
                //_host.TimeCodeModeChangedEvent += TimeCodeModeChangedEvent;
                //_host.DownConvertedHDVideoModeChangedEvent += DownConvertedHDVideoModeChangedEvent;
                //_host.MethodForDownConvertedSDChangedEvent += MethodForDownConvertedSDChangedEvent;
                //_host.MultiViewVideoModeChangedEvent += MultiViewVideoModeChangedEvent;
                //_host.TypeAutoVideoModeChangedEvent += TypeAutoVideoModeChangedEvent;
                //_host.VideoModeChangedEvent += VideoModeChangedEvent;
            }
        }

        public override void VideoModeChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("VideoModeChangedEvent", null);
        }

        public override void TypeAutoVideoModeChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("TypeAutoVideoModeChanged", null);
        }

        public override void MultiViewVideoModeChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("MultiViewVideoModeChangedEvent", null);
        }

        public override void MethodForDownConvertedSDChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("MethodForDownConvertedSDChangedEvent", null);
        }

        public override void DownConvertedHDVideoModeChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("DownConvertedHDVideoModeChangedEvent", null);
        }

        public override void TimeCodeModeChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("TimeCodeModeChanged", null);
        }

        public override void TimeCodeLockedChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("TimeCodeLockedChanged", null);
        }

        public override void TimeCodeChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("TimeCodeChanged", null);
        }

        public override void SuperSourceCascadeChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("SuperSourceCascadeChanged", null);
        }

        public override void SDI3GOutputLevelChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("SDI3GOutputLevelChangedEvent", null);
        }

        public override void PowerStatusChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("PowerStatusChangedEvent", null);
        }

        public override void AutoVideoModeDetectedChangedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("AutoVideoModeDetectedChanged", null);
        }

        public override void DisconnectedEvent(object sender, EventArgs e)
        {
            _hub.Clients.All.SendCoreAsync("DisconnectedEvent", null);
        }
    }
}