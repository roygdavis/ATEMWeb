using System;
using System.Runtime.Serialization;

namespace Atem.Hosts.Legacy
{
    [DataContract]
    public class MixEffectsEventArgs : EventArgs
    {
        [DataMember]
        public long Input { get; set; }

        [DataMember]
        public MixEffectBuses MEBus { get; set; }

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
