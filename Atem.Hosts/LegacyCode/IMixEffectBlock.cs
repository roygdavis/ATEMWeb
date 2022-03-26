using Atem.Hosts.Legacy;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atem.Hosts.Legacy.Interfaces
{
    public interface IMixEffectBlock : IDisposable
    {
        int MEBlockIndex { get; set; }
        long ProgramInput { get; set; }
        long PreviewInput { get; set; }
        int PreviewLive { get; set; }
        int PreviewTransition { get; set; }
        int InTransition { get; set; }
        double TransitionPosition { get; set; }
        uint TransitionFramesRemaining { get; set; }
        uint FadeToBlackRate { get; set; }
        uint FadeToBlackFramesRemaining { get; set; }
        int FadeToBlackFullyBlack { get; set; }
        int InFadeToBlack { get; set; }
        int FadeToBlackInTransition { get; set; }
        _BMDSwitcherInputAvailability InputAvailabilityMask { get; set; }
        event EventHandler<MixEffectsEventArgs> ProgramInputChanged;
        event EventHandler<MixEffectsEventArgs> PreviewInputChanged;
        event EventHandler<MixEffectsEventArgs> TransitionFramesRemainingChanged;
        event EventHandler<MixEffectsEventArgs> TransitionPositionChanged;
        event EventHandler<MixEffectsEventArgs> InTransitionChanged;
        event EventHandler<MixEffectsEventArgs> InFadeToBlackChanged;
        event EventHandler<MixEffectsEventArgs> FadeToBlackFramesRemainingChanged;
        event EventHandler<MixEffectsEventArgs> PreviewLiveChanged;
        event EventHandler<MixEffectsEventArgs> PreviewTransitionChanged;
        event EventHandler<MixEffectsEventArgs> FadeToBlackRateChanged;
        event EventHandler<MixEffectsEventArgs> FadeToBlackFullyBlackChanged;
        event EventHandler<MixEffectsEventArgs> FadeToBlackInTransitionChanged;
        event EventHandler<MixEffectsEventArgs> InputAvailabilityMaskChanged;
    }
}