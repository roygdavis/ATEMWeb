using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SixteenMedia.ATEM.Wrapper
{
    [DataContract]
    public enum AtemMixEffectsEventTypes
    {
        [EnumMember(Value = "PGMChanged")]
        PGMChanged,

        [EnumMember(Value = "PVWChanged")]
        PVWChanged,

        [EnumMember(Value = "TransitionFramesRemainingChanged")]
        TransitionFramesRemainingChanged,

        [EnumMember(Value = "TransitionPositionChanged")]
        TransitionPositionChanged,

        [EnumMember(Value = "InTransitionChanged")]
        InTransitionChanged,

        [EnumMember(Value = "InFadeToBlackChanged")]
        InFadeToBlackChanged,

        [EnumMember(Value = "FadeToBlackFramesRemainingChanged")]
        FadeToBlackFramesRemainingChanged,

        [EnumMember(Value = "PreviewLiveChanged")]
        PreviewLiveChanged,

        [EnumMember(Value = "PreviewTransitionChanged")]
        PreviewTransitionChanged,

        [EnumMember(Value = "AvailabilityMaskChanged")]
        AvailabilityMaskChanged,

        [EnumMember(Value = "FadeToBlackRateChanged")]
        FadeToBlackRateChanged,

        [EnumMember(Value = "FadeToBlackFullyBlackChanged")]
        FadeToBlackFullyBlackChanged,

        [EnumMember(Value = "FadeToBlackInTransitionChanged")]
        FadeToBlackInTransitionChanged,

        [EnumMember(Value = "InputAvailabilityMaskChanged")]
        InputAvailabilityMaskChanged
    }
}
