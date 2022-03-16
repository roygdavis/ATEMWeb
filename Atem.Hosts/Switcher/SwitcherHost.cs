using Atem.Hosts.Exceptions;
using Atem.Hosts.MixEffects;
using Atem.Hosts.Notifiers;
using BMDSwitcherAPI;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace Atem.Hosts.Switcher
{
    public class SwitcherHost : ISwitcherHost
    {
        private IBMDSwitcher? _switcher;
        private readonly ISwitcherNotifier _notifier;
        private readonly ILogger<ISwitcherHost> _logger;

        public Dictionary<int, IMixEffectsHost> MixEffects { get; set; }

        public SwitcherHost(ISwitcherNotifier notifier, ILogger<ISwitcherHost> logger)
        {
            _switcher = null;
            _logger = logger;
            _notifier = notifier;
            MixEffects = new Dictionary<int, IMixEffectsHost>();
        }

        public async Task Connect(string address)
        {
            MixEffects = new Dictionary<int, IMixEffectsHost>();
            _BMDSwitcherConnectToFailure failReason = 0;

            try
            {
                IBMDSwitcherDiscovery _switcherDiscovery = new CBMDSwitcherDiscovery();
                // Note that ConnectTo() can take several seconds to return, both for success or failure,
                // depending upon hostname resolution and network response times, so it may be best to
                // do this in a separate thread to prevent the main GUI thread blocking.
                await Task.Run(() => _switcherDiscovery.ConnectTo(address, out _switcher, out failReason));
            }
            catch (COMException comEx)
            {
                // An exception will be thrown if ConnectTo fails. For more information, see failReason.
                _logger.LogError(comEx, $"Failed to connect to switcher at {address}");
                switch (failReason)
                {
                    case _BMDSwitcherConnectToFailure.bmdSwitcherConnectToFailureNoResponse:
                        // MessageBox.Show("No response from Switcher", "Error");
                        throw new ConnectFailureNoResponseException(string.Format("Cannot connect to switcher at address: {0}", address));
                    case _BMDSwitcherConnectToFailure.bmdSwitcherConnectToFailureIncompatibleFirmware:
                        // MessageBox.Show("Switcher has incompatible firmware", "Error");
                        throw new ConnectFailureIncompatibleFirmwareException(string.Format("Switcher at address {0} has incompatible firmware.  Either rebuild this library to the switcher version or upgrade the switcher firmware to match.", address));
                    default:
                        throw new Exception(string.Format("Cannot connect to switcher at address {0}.  See inner exception for more information", address), comEx);
                }
                throw new Exception(string.Format("Cannot connect to switcher at address {0}.  See inner exception for more information", address), comEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to connect to switcher at {address}");
            }

            if (_switcher != null)
            {
                // Get the switcher name:
                await Task.Run(async () =>
                {
                    _switcher.GetProductName(out string switcherName);
                    await _notifier.SwitcherConnected(switcherName, address);
                });

                // Install SwitcherMonitor callbacks:
                _switcher.AddCallback(_notifier);

                await Task.Run(() =>
                {
                    // init the mix effects count
                    int meIndex = 0;

                    // meIteratorIID holds the COM class GUID of the iterator
                    Guid meIteratorIID = typeof(IBMDSwitcherMixEffectBlockIterator).GUID;

                    // meIteratorPtr holds the pointer to the mix effects iterator object
                    // create the iterator and out to the meIteratorPtr pointer
                    _switcher.CreateIterator(ref meIteratorIID, out IntPtr meIteratorPtr);

                    // create the iterator
#pragma warning disable CA1416 // Validate platform compatibility
                    IBMDSwitcherMixEffectBlockIterator meIterator = (IBMDSwitcherMixEffectBlockIterator)Marshal.GetObjectForIUnknown(meIteratorPtr);
#pragma warning restore CA1416 // Validate platform compatibility

                    if (meIterator != null)
                    {
                        // now we can start to iterate over the ME blocks this ATEM has. Usually the basic ATEM's have only one Mix Effect Block.  The 2M/E ATEM's have two.  Some have more.
                        // try and get the Mix Effect Block from the iterator
                        meIterator.Next(out IBMDSwitcherMixEffectBlock meBlock);

                        // if that wasn't null then add to our array of ME Blocks
                        while (meBlock != null)
                        {
                            meBlock.AddCallback(_notifier);
                            var mixEffectBlock = new MixEffectsHost(this, meBlock, _notifier);
                            MixEffects.Add(meIndex, mixEffectBlock);

                            // TODO: raise an event
                            // the consumer of this event should hook into the mbBlock events
                            //MixEffectBlockConnectedEvent?.Invoke(this, new MixEffectBlockConnectedEventArgs(mixEffectBlock));

                            // Try and get the next block.  A ref of null means there are no more Mix Effects Blocks on this ATEM
                            meIterator.Next(out meBlock);

                            // We increment our Mix Effect count!
                            meIndex++;
                        }

                        // Now do a keyer discovery on each mixeffects
                        foreach (var item in MixEffects)
                        {
                            item.Value.DiscoverKeyers();
                        }
                    }
                });
            }
        }
    }
}