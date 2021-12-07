using System;

namespace SixteenMedia.ATEM.Wrapper
{
    public class MixEffectBlockConnectedEventArgs:EventArgs
    {
        public MEBlock ConnectedMEBlock { get; set; }

        public MixEffectBlockConnectedEventArgs(MEBlock connectedMEBlock)
        {
            ConnectedMEBlock = connectedMEBlock;
        }
    }
}