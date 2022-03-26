using Atem.Hosts.Legacy.Interfaces;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Legacy.Hosts.MixEffects
{
    public class MixEffectsHost : IMixEffectsHost, IDisposable
    {
        private readonly IBMDSwitcher _switcher;
        private bool disposedValue;

        public List<IMixEffectBlock> MixEffectsBlocks { get; set; }

        public MixEffectsHost(IBMDSwitcher switcher)
        {
            _switcher = switcher;
            nullifyMixEffectsBlocks();
        }

        public bool HasMixEffectBlocks { get => !(MixEffectsBlocks is null || !MixEffectsBlocks.Any() || MixEffectsBlocks.Count < 1); }

        private void nullifyMixEffectsBlocks()
        {
            if (MixEffectsBlocks != null && MixEffectsBlocks.Any())
            {
                foreach (var meBlock in MixEffectsBlocks)
                {
                    if (meBlock != null)
                    {
                        // Dispose reference
                        meBlock.Dispose();
                    }
                }
            }
            MixEffectsBlocks = new List<IMixEffectBlock>();
        }

        /// <summary>
        /// Creates the BMD iterator and retrives the Mix Effect(s) banks from the ATEM
        /// </summary>
        public void DiscoverMixEffects()
        {
            // ensure m_mixEffectBlocks is empty
            nullifyMixEffectsBlocks();

            // init the mix effects count
            int meCount = 0;

            // meIteratorIID holds the COM class GUID of the iterator
            Guid meIteratorIID = typeof(IBMDSwitcherMixEffectBlockIterator).GUID;

            // meIteratorPtr holds the pointer to the mix effects iterator object
            // create the iterator and out to the meIteratorPtr pointer
            _switcher.CreateIterator(ref meIteratorIID, out IntPtr meIteratorPtr);

            // create the iterator
#pragma warning disable CA1416 // Validate platform compatibility
            IBMDSwitcherMixEffectBlockIterator meIterator = (IBMDSwitcherMixEffectBlockIterator)Marshal.GetObjectForIUnknown(meIteratorPtr);
#pragma warning restore CA1416 // Validate platform compatibility

            // bail if that returned null
            if (meIterator == null)
                return;

            // now we can start to iterate over the ME blocks this ATEM has. Usually the basic ATEM's have only one Mix Effect Block.  The 2M/E ATEM's have two.  Some have more.
            // try and get the Mix Effect Block from the iterator
            meIterator.Next(out IBMDSwitcherMixEffectBlock meBlock);

            // if that wasn't null then add to our array of ME Blocks
            while (meBlock != null)
            {
                var mixEffectBlock = new MixEffectBlock(meBlock, meCount);
                MixEffectsBlocks.Add(mixEffectBlock);

                // raise an event
                // the consumer of this event should hook into the mbBlock events
                // TODO: Bind the events to the Hub class
                //MixEffectBlockConnectedEvent?.Invoke(this, new MixEffectBlockConnectedEventArgs(mixEffectBlock));

                // Try and get the next block.  A ref of null means there are no more Mix Effects Blocks on this ATEM
                meIterator.Next(out meBlock);

                // We increment our Mix Effect count!
                meCount++;
            }

        }

        private void Foo()
        {
            //MixEffectsBlocks.First().ProgramInput;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                nullifyMixEffectsBlocks();
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MixEffectsHost()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
