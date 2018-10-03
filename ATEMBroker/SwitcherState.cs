using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixteenMedia.ATEM.Broker.BMDSwitcherAPI;
using System.Configuration;

namespace SixteenMedia.ATEM.Broker
{
    class SwitcherState
    {
        public MEState MixEffects1 { get; set; }
        public MEState MixEffects2 { get; set; }

        public SwitcherState(ref IBMDSwitcherMixEffectBlock meBlock1, ref IBMDSwitcherMixEffectBlock meBlock2)
        {
            this.MixEffects1 = new MEState(ref meBlock1);
            this.MixEffects2 = new MEState(ref meBlock2);
        }


    }




}