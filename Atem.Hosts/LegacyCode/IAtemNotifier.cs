using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Legacy.Notifiers
{
    public interface IAtemNotifier<T>
    {
        void VideoModeChangedEvent(T arg);
        void MethodForDownConvertedSDChangedEvent(T arg);
        void DownConvertedHDVideoModeChangedEvent(T arg);
        void MultiViewVideoModeChangedEvent(T arg);
        void PowerStatusChangedEvent(T arg);
        void DisconnectedEvent(T arg);
        void SDI3GOutputLevelChangedEvent(T arg);
        void TypeAutoVideoModeChangedEvent(T arg);
        void AutoVideoModeDetectedChangedEvent(T arg);
        void SuperSourceCascadeChangedEvent(T arg);
        void TimeCodeChangedEvent(T arg);
        void TimeCodeLockedChangedEvent(T arg);
        void TimeCodeModeChangedEvent(T arg);
        void ProgramInputChanged(T arg);
        void PreviewInputChanged(T arg);
        void TransitionFramesRemainingChanged(T arg);
        void TransitionPositionChanged(T arg);
        void InTransitionChanged(T arg);
        void InFadeToBlackChanged(T arg);
        void FadeToBlackFramesRemainingChanged(T arg);
        void PreviewLiveChanged(T arg);
        void PreviewTransitionChanged(T arg);
        void FadeToBlackRateChanged(T arg);
        void FadeToBlackFullyBlackChanged(T arg);
        void FadeToBlackInTransitionChanged(T arg);
        void InputAvailabilityMaskChanged(T arg);
    }
}