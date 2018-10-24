using System;
using System.Runtime.Serialization;

namespace SixteenMedia.ATEM.Wrapper
{
    [DataContract]
    public class MixEffectsEventArgs : EventArgs
    {
        [DataMember]
        public long Input { get; set; }

        [DataMember]
        public MEBuses MEBus { get; set; }

        [DataMember]
        public uint TransitionFramesRemaining { get; set; }

        [DataMember]
        public double TransitionPosition { get; set; }

        [DataMember]
        public int InTransition { get; set; }

        [DataMember]
        public int MixEffectsIndex { get; set; }

        [DataMember]
        public AtemMixEffectsEventTypes MixEffectsEventType { get; set; }
    }   
}
