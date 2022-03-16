using System;

namespace ATEM.Services.Eventing.SwitcherEventing
{
    public abstract class SwitcherEventingByHubBase
    {
        public abstract void AutoVideoModeDetectedChangedEvent(object sender, EventArgs e);
        public abstract void DisconnectedEvent(object sender, EventArgs e);
        public abstract void DownConvertedHDVideoModeChangedEvent(object sender, EventArgs e);
        public abstract void MethodForDownConvertedSDChangedEvent(object sender, EventArgs e);
        public abstract void MultiViewVideoModeChangedEvent(object sender, EventArgs e);
        public abstract void PowerStatusChangedEvent(object sender, EventArgs e);
        public abstract void SDI3GOutputLevelChangedEvent(object sender, EventArgs e);
        public abstract void SuperSourceCascadeChangedEvent(object sender, EventArgs e);
        public abstract void TimeCodeChangedEvent(object sender, EventArgs e);
        public abstract void TimeCodeLockedChangedEvent(object sender, EventArgs e);
        public abstract void TimeCodeModeChangedEvent(object sender, EventArgs e);
        public abstract void TypeAutoVideoModeChangedEvent(object sender, EventArgs e);
        public abstract void VideoModeChangedEvent(object sender, EventArgs e);
    }
}