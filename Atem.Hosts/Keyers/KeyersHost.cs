using Atem.Hosts.MixEffects;
using Atem.Hosts.Notifiers;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Keyers
{
    public class KeyersHost : IKeyersHost
    {
        private IBMDSwitcherKey? _keyer;
        private bool disposedValue;
        private ISwitcherNotifier _notifier;

        public IMixEffectsHost MixEffectsHost { get; set; }

        public KeyersHost(IMixEffectsHost mixEffectsHost, IBMDSwitcherKey keyer, ISwitcherNotifier notifier)
        {
            _keyer = keyer;
            MixEffectsHost = mixEffectsHost;
            _notifier = notifier;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                if (_keyer != null)
                    _keyer.RemoveCallback(_notifier);
                _keyer = null;
                disposedValue = true;
            }
        }

        // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~KeyersHost()
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

        public void Foo()
        {
            _keyer.CanBeDVEKey(out int canDVE);
            _keyer.DoesSupportAdvancedChroma(out int supportsAdvancedChroma);
            _keyer.GetCutInputAvailabilityMask(out _BMDSwitcherInputAvailability availabilityMask);
            _keyer.GetFillInputAvailabilityMask(out _BMDSwitcherInputAvailability availabilityMask);
            _keyer.GetInputCut(out long input);
            _keyer.GetInputFill(out long input);
            _keyer.GetMaskBottom(out double bottom);
            _keyer.GetMasked(out int maskEnabled);
            _keyer.GetMaskLeft(out double left);
            _keyer.GetMaskRight(out double right);
            _keyer.GetMaskTop(out double top);
            _keyer.GetOnAir(out int onAir);
            _keyer.GetTransitionSelectionMask(out _BMDSwitcherTransitionSelection selectionMask);
            _keyer.ResetMask();
            _keyer.SetInputCut(long input);
            _keyer.SetInputFill(long input);
            _keyer.SetMaskBottom(double bottom);
            _keyer.SetMasked(int maskEnabled);
            _keyer.SetMaskLeft(double left);
            _keyer.SetMaskRight(double right);
            _keyer.SetMaskTop(double top);
            _keyer.SetOnAir(int onAir);
            _keyer.SetType(_BMDSwitcherKeyType type);
        }
    }
}