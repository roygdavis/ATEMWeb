using System;

namespace Atem.Hosts.Legacy
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