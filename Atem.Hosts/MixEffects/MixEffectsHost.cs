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
        private IBMDSwitcherMixEffectBlock? _mixEffects;
        private bool disposedValue;
        private readonly ISwitcherNotifier _notifier;

        public ISwitcherHost SwitcherHost { get; set; }
        public Dictionary<int, IKeyersHost> KeyersHosts { get; set; }

        public MixEffectsHost(ISwitcherHost switcher, IBMDSwitcherMixEffectBlock mixEffects, ISwitcherNotifier notifer)
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
                IntPtr keyerIteratorPtr = IntPtr.Zero;
                _mixEffects?.CreateIterator(ref keyerIteratorIID, out keyerIteratorPtr);

                IBMDSwitcherKeyIterator? keyerIterator = null;
                if (keyerIteratorPtr != IntPtr.Zero)
                {
                    // create the iterator
#pragma warning disable CA1416 // Validate platform compatibility
                    keyerIterator = (IBMDSwitcherKeyIterator)Marshal.GetObjectForIUnknown(keyerIteratorPtr);
#pragma warning restore CA1416 // Validate platform compatibility
                }

                if (keyerIterator != null)
                {
                    // now we can start to iterate over the ME blocks this ATEM has. Usually the basic ATEM's have only one Mix Effect Block.  The 2M/E ATEM's have two.  Some have more.
                    // try and get the Mix Effect Block from the iterator
                    keyerIterator.Next(out IBMDSwitcherKey keyer);

                    // if that wasn't null then add to our array of ME Blocks
                    while (keyer != null)
                    {
                        keyer.AddCallback(_notifier);
                        var keyerHost = new KeyersHost(this, keyer, _notifier);
                        KeyersHosts.Add(keyerIndex, keyerHost);

                        // Try and get the next block.  A ref of null means there are no more Keyers on this ATEM
                        keyerIterator.Next(out keyer);

                        // We increment our Mix Effect count!
                        keyerIndex++;
                    }
                }
            });
        }

        public async Task<long> GetProgramInput()
        {
            return await Task.Run(() =>
            {
                var input = default(long);
                _mixEffects?.GetProgramInput(out input);
                return input;
            });
        }

        public async Task SetProgramInput(long input)
        {
            await Task.Run(() => _mixEffects?.SetProgramInput(input));
        }

        public async Task<long> GetPreviewInput()
        {
            return await Task.Run(() =>
            {
                var input = default(long);
                _mixEffects?.GetPreviewInput(out input);
                return input;
            });
        }

        public async Task SetPreviewInput(long input)
        {
            await Task.Run(() => _mixEffects?.SetPreviewInput(input));
        }

        public Task GetPreviewLive()
        {
            throw new NotImplementedException();
        }

        public Task GetPreviewTransition()
        {
            throw new NotImplementedException();
        }

        public Task SetPreviewTransition()
        {
            throw new NotImplementedException();
        }

        public Task PerformAutoTransition()
        {
            throw new NotImplementedException();
        }

        public Task PerformCut()
        {
            throw new NotImplementedException();
        }

        public Task GetInTransition()
        {
            throw new NotImplementedException();
        }

        public Task GetTransitionPosition()
        {
            throw new NotImplementedException();
        }

        public Task SetTransitionPosition()
        {
            throw new NotImplementedException();
        }

        public Task GetTransitionFramesRemaining()
        {
            throw new NotImplementedException();
        }

        public Task PerformFadeToBlack()
        {
            throw new NotImplementedException();
        }

        public Task GetFadeToBlackRate()
        {
            throw new NotImplementedException();
        }

        public Task SetFadeToBlackRate()
        {
            throw new NotImplementedException();
        }

        public Task GetFadeToBlackFramesRemaining()
        {
            throw new NotImplementedException();
        }

        public Task GetFadeToBlackFullyBlack()
        {
            throw new NotImplementedException();
        }

        public Task SetFadeToBlackFullyBlack()
        {
            throw new NotImplementedException();
        }

        public Task GetInFadeToBlack()
        {
            throw new NotImplementedException();
        }

        public Task GetFadeToBlackInTransition()
        {
            throw new NotImplementedException();
        }

        public Task GetInputAvailabilityMask()
        {
            throw new NotImplementedException();
        }

        #region Disposal
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    foreach (var item in KeyersHosts)
                    {
                        item.Value.Dispose();
                    }
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                if (_mixEffects != null && _notifier != null)
                    _mixEffects?.RemoveCallback(_notifier);
                _mixEffects = null;
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~MixEffectsHost()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}