using Atem.Hosts.Core;
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
        private bool disposedValue;
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

        public async Task AllowStreamingToResume()
        {
            await Task.Run(() => _switcher?.AllowStreamingToResume());
        }

        public async Task<bool> DoesSupportAutoVideoMode()
        {
            int supported = 0;
            return await Task.Run<int>(() => { _switcher?.DoesSupportAutoVideoMode(out supported); return supported; }) == 1;
        }

        public async Task<bool> DoesSupportDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            int supported = 0;
            return await Task.Run<int>(() => { _switcher?.DoesSupportDownConvertedHDVideoMode(from, to, out supported); return supported; }) == 1;
        }

        public async Task<bool> DoesSupportMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            int supported = 0;
            return await Task.Run<int>(() => { _switcher?.DoesSupportMultiViewVideoMode(from, to, out supported); return supported; }) == 1;
        }

        public async Task<bool> DoesSupportVideoMode(_BMDSwitcherVideoMode mode)
        {
            int supported = 0;
            return await Task.Run<int>(() => { _switcher?.DoesSupportVideoMode(mode, out supported); return supported; }) == 1;
        }

        public async Task<bool> DoesVideoModeChangeRequireReconfiguration(_BMDSwitcherVideoMode mode)
        {
            int supported = 0;
            return await Task.Run<int>(() => { _switcher?.DoesVideoModeChangeRequireReconfiguration(mode, out supported); return supported; }) == 1;
        }

        public async Task<_BMDSwitcher3GSDIOutputLevel> Get3GSDIOutputLevel()
        {
            return await Task.Run<_BMDSwitcher3GSDIOutputLevel>(() =>
                {
                    _BMDSwitcher3GSDIOutputLevel level = default;
                    _switcher?.Get3GSDIOutputLevel(out level);
                    return level;
                });
        }

        public async Task<string> GetProductName()
        {
            return await Task.Run<string>(() =>
            {
                var productName = default(string);
                _switcher?.GetProductName(out productName);
                return productName ?? string.Empty;
            });
        }
        public async Task<_BMDSwitcherVideoMode> GetVideoMode()
        {
            return await Task.Run<_BMDSwitcherVideoMode>(() =>
            {
                _BMDSwitcherVideoMode mode = default;
                _switcher?.GetVideoMode(out mode);
                return mode;
            });
        }


        public async Task<_BMDSwitcherDownConversionMethod> GetMethodForDownConvertedSD()
        {
            return await Task.Run<_BMDSwitcherDownConversionMethod>(() =>
            {
                _BMDSwitcherDownConversionMethod mode = default;
                _switcher?.GetMethodForDownConvertedSD(out mode);
                return mode;
            });
        }
        public async Task<_BMDSwitcherVideoMode> GetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from)
        {
            return await Task.Run<_BMDSwitcherVideoMode>(() =>
            {
                _BMDSwitcherVideoMode mode = default;
                _switcher?.GetDownConvertedHDVideoMode(from, out mode);
                return mode;
            });
        }

        public async Task<_BMDSwitcherVideoMode> GetMultiViewVideoMode(_BMDSwitcherVideoMode from) 
        {
            return await Task.Run<_BMDSwitcherVideoMode>(() =>
            {
                _BMDSwitcherVideoMode mode = default;
                _switcher?.GetMultiViewVideoMode(from, out mode);
                return mode;
            });
        }

        public async Task<_BMDSwitcherPowerStatus> GetPowerStatus()
        {
            return await Task.Run<_BMDSwitcherPowerStatus>(() =>
            {
                _BMDSwitcherPowerStatus mode = default;
                _switcher?.GetPowerStatus(out mode);
                return mode;
            });
        }

        public async Task<TimeCode> GetTimeCode() 
        {
            return await Task.Run<TimeCode>(() =>
            {
                byte hours = default(byte);
                byte minutes = default(byte);
                byte seconds = default(byte);
                byte frames = default(byte);
                int dropFrame = default(int);
                _switcher?.GetTimeCode(out  hours, out  minutes, out  seconds, out  frames, out dropFrame);
                return new TimeCode(hours, minutes, seconds, frames, dropFrame);
            });
        }

        public async Task<bool> GetTimeCodeLocked()
        {
            return await Task.Run<bool>(() =>
            {
                int mode = default;
                _switcher?.GetTimeCodeLocked(out mode);
                return mode == 1;
            });
        }

        public async Task<_BMDSwitcherTimeCodeMode> GetTimeCodeMode() 
        {
            return await Task.Run<_BMDSwitcherTimeCodeMode>(() =>
            {
                _BMDSwitcherTimeCodeMode mode = default;
                _switcher?.GetTimeCodeMode(out mode);
                return mode;
            });
        }

        public async Task<bool> GetAreOutputsConfigurable()
        {
            return await Task.Run<bool>(() =>
            {
                int mode = default;
                _switcher?.GetAreOutputsConfigurable(out mode);
                return mode == 1;
            });
        }

        public async Task<bool> GetSuperSourceCascade()
        {
            return await Task.Run<bool>(() =>
            {
                int mode = default;
                _switcher?.GetSuperSourceCascade(out mode);
                return mode == 1;
            });
        }

        public async Task<bool> GetAutoVideoMode() 
        {
            return await Task.Run<bool>(() =>
            {
                int mode = default;
                _switcher?.GetAutoVideoMode(out mode);
                return mode == 1;
            });
        }

        public async Task<bool> GetAutoVideoModeDetected() 
        {
            return await Task.Run<bool>(() =>
            {
                int mode = default;
                _switcher?.GetAutoVideoModeDetected(out mode);
                return mode == 1;
            });
        }

        public async Task SetMethodForDownConvertedSD(_BMDSwitcherDownConversionMethod method) => await Task.Run(() => _switcher?.SetMethodForDownConvertedSD(method));

        public async Task SetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to) => await Task.Run(() => _switcher?.SetDownConvertedHDVideoMode(from, to));

        public async Task SetMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to) => await Task.Run(() => _switcher?.SetMultiViewVideoMode(from, to));

        public async Task Set3GSDIOutputLevel(_BMDSwitcher3GSDIOutputLevel level) => await Task.Run(() => _switcher?.Set3GSDIOutputLevel(level));

        public async Task SetTimeCode(TimeCode timeCode) => await Task.Run(() => _switcher?.SetTimeCode(timeCode.Hours, timeCode.Minutes, timeCode.Seconds, timeCode.Frames));

        public async Task RequestTimeCode() => await Task.Run(() => _switcher?.RequestTimeCode());

        public async Task SetTimeCodeMode(_BMDSwitcherTimeCodeMode mode) => await Task.Run(() => _switcher?.SetTimeCodeMode(mode));

        public async Task SetSuperSourceCascade(int cascade) => await Task.Run(() => _switcher?.SetSuperSourceCascade(cascade));

        public async Task SetAutoVideoMode(bool enabled) => await Task.Run(() => _switcher?.SetAutoVideoMode(Convert.ToInt32(enabled)));

        public async Task SetVideoMode(_BMDSwitcherVideoMode mode) => await Task.Run(() => _switcher?.SetVideoMode(mode));

        #region Disposal
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    if (MixEffects is not null)
                        foreach (var item in MixEffects)
                        {
                            item.Value.Dispose();
                        }
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                if (_notifier != null && _switcher != null)
                    _switcher.RemoveCallback(_notifier);
                _switcher = null;
                disposedValue = true;
            }
        }

        ~SwitcherHost()
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