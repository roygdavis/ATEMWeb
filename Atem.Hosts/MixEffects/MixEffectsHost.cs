using Atem.Hosts.Keyers;
using Atem.Hosts.Notifiers;
using Atem.Hosts.Switcher;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.MixEffects
{
    public class MixEffectsHost : IMixEffectsHost
    {
        private IBMDSwitcherMixEffectBlock _mixEffects;
        private readonly ISwitcherNotifier _notifier;

        public ISwitcherHost SwitcherHost { get; set; }
        public Dictionary<int, IKeyersHost> KeyersHosts { get; set; }

        public MixEffectsHost(ISwitcherHost switcher, IBMDSwitcherMixEffectBlock mixEffects,ISwitcherNotifier notifer)
        {
            SwitcherHost = switcher;
            _mixEffects = mixEffects;
            _notifier = notifer;
            KeyersHosts = new Dictionary<int, IKeyersHost>();
        }

        public async Task DiscoverKeyers()
        {
            KeyersHosts.Clear();
            await Task.Run(() =>
            {
                // init the mix effects count
                int keyerIndex = 0;

                // meIteratorIID holds the COM class GUID of the iterator
                Guid keyerIteratorIID = typeof(IBMDSwitcherKeyIterator).GUID;

                // meIteratorPtr holds the pointer to the mix effects iterator object
                // create the iterator and out to the meIteratorPtr pointer
                _mixEffects.CreateIterator(ref keyerIteratorIID, out IntPtr keyerIteratorPtr);

                // create the iterator
#pragma warning disable CA1416 // Validate platform compatibility
                IBMDSwitcherKeyIterator keyerIterator = (IBMDSwitcherKeyIterator)Marshal.GetObjectForIUnknown(keyerIteratorPtr);
#pragma warning restore CA1416 // Validate platform compatibility

                if (keyerIterator != null)
                {
                    // now we can start to iterate over the ME blocks this ATEM has. Usually the basic ATEM's have only one Mix Effect Block.  The 2M/E ATEM's have two.  Some have more.
                    // try and get the Mix Effect Block from the iterator
                    keyerIterator.Next(out IBMDSwitcherKey keyer);

                    // if that wasn't null then add to our array of ME Blocks
                    while (keyer != null)
                    {
                        keyer.AddCallback(_notifier);
                        var keyerHost = new KeyersHost(this, keyer);
                        KeyersHosts.Add(keyerIndex, keyerHost);

                        // Try and get the next block.  A ref of null means there are no more Keyers on this ATEM
                        keyerIterator.Next(out keyer);

                        // We increment our Mix Effect count!
                        keyerIndex++;
                    }
                }
            });
        }

        public async Task SetPGM(long input)
        {
            await Task.Run(() => _mixEffects.SetProgramInput(input));
        }

        public async Task SetPVW(long input)
        {
            await Task.Run(() => _mixEffects.SetPreviewInput(input));
        }
    }
}