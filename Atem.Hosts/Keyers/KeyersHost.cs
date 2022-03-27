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
    }
}