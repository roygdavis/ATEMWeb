using System;

namespace ATEM.Services
{
    public class MixEffectBlockConnectedEventArgs:EventArgs
    {
        public MixEffectBlock ConnectedMEBlock { get; set; }

        public MixEffectBlockConnectedEventArgs(MixEffectBlock connectedMEBlock)
        {
            ConnectedMEBlock = connectedMEBlock;
        }
    }
}