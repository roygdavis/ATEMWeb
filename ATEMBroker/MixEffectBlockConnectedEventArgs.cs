using System;
using SixteenMedia.ATEM.Broker.BMDSwitcherAPI;

namespace SixteenMedia.ATEM.Broker
{
    public class MixEffectBlockConnectedEventArgs:EventArgs
    {
        public Extensions.MEBlock ConnectedMEBlock { get; set; }

        public MixEffectBlockConnectedEventArgs(Extensions.MEBlock connectedMEBlock)
        {
            ConnectedMEBlock = connectedMEBlock;
        }
    }
}