using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenMedia.ATEM.Wrapper
{
    public class MixEffectsEventArgs : EventArgs
    {
        public long Input { get; set; }
        public MEBuses MEBus { get; set; }
        public uint TransitionFramesRemaining { get; set; }
        public double TransitionPosition { get; set; }
        public int InTransition { get; set; }
    }   
}
