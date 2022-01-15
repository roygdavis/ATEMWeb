using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Runtime.Serialization;
using BMDSwitcherAPI;
using ATEM.Services.Interfaces;
using ATEM.Services.Exceptions;
using ATEM.Services.Hosts.MixEffects;
using ATEM.Services.Hubs;

namespace ATEM.Services.Hosts
{
    [DataContract]
    public class SwitcherHost : ISwitcherHost, IDisposable, IBMDSwitcherCallback
    {
        #region private fields
        /// <summary>
        /// Holds the switcher enumerator object
        /// </summary>
        private readonly IBMDSwitcherDiscovery _switcherDiscovery;
        private readonly ATEMEventsHub _hub;

        /// <summary>
        /// Holds the switcher object
        /// </summary>
        private IBMDSwitcher _switcher;
        #endregion

        #region Public properties

        [DataMember]
        public string AtemIPAddress { get; set; }

        [DataMember]
        public string SwitcherName { get; set; }

        [DataMember]
        public bool IsConnected { get; set; }

        [DataMember]
        public _BMDSwitcherVideoMode VideoMode
        {
            get
            {
                if (_switcher != null)
                {
                    _switcher.GetVideoMode(out _BMDSwitcherVideoMode v);
                    return v;
                }
                throw new NullReferenceException("m_switcher is null");
            }
            set
            {
                _switcher.SetVideoMode(value);
            }
        }

        [DataMember]
        public string ProductName
        {
            get
            {
                if (_switcher != null)
                {
                    _switcher.GetProductName(out string v);
                    return v;
                }
                throw new NullReferenceException("m_switcher is null");
            }
            // empty setter so that this property can be serialised
            set { }

        }

        [DataMember]
        public _BMDSwitcher3GSDIOutputLevel SDI3GOutputLevel
        {
            get
            {
                if (_switcher != null)
                {
                    _switcher.Get3GSDIOutputLevel(out _BMDSwitcher3GSDIOutputLevel v);
                    return v;
                }
                throw new NullReferenceException("m_switcher is null");
            }
            set
            {
                _switcher.Set3GSDIOutputLevel(value);
            }
        }

        [DataMember]
        public _BMDSwitcherPowerStatus PowerStatus
        {
            get
            {
                if (_switcher != null)
                {
                    _switcher.GetPowerStatus(out _BMDSwitcherPowerStatus v);
                    return v;
                }
                throw new NullReferenceException("m_switcher is null");
            }
            // empty setter so that this property can be serialised
            set { }
        }
        #endregion

        #region Constructors
        public SwitcherHost(CBMDSwitcherDiscovery switcherDiscovery, ATEMEventsHub hub)
        {
            _switcherDiscovery = switcherDiscovery;
            _hub = hub;
            SwitcherDisconnected();		// start with switcher disconnected
        }
        #endregion

        public void Notify(_BMDSwitcherEventType eventType, _BMDSwitcherVideoMode coreVideoMode)
        {
            // TODO: Each event should provide the value from the _switcher.GetXXX method
            switch (eventType)
            {
                case _BMDSwitcherEventType.bmdSwitcherEventTypeVideoModeChanged:
                    _hub.Clients.All.SendCoreAsync("VideoModeChangedEvent", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeMethodForDownConvertedSDChanged:
                    _hub.Clients.All.SendCoreAsync("MethodForDownConvertedSDChangedEvent", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeDownConvertedHDVideoModeChanged:
                    _hub.Clients.All.SendCoreAsync("DownConvertedHDVideoModeChangedEvent", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeMultiViewVideoModeChanged:
                    _hub.Clients.All.SendCoreAsync("MultiViewVideoModeChangedEvent", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypePowerStatusChanged:
                    _hub.Clients.All.SendCoreAsync("PowerStatusChangedEvent", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeDisconnected:
                    _hub.Clients.All.SendCoreAsync("DisconnectedEvent", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventType3GSDIOutputLevelChanged:
                    _hub.Clients.All.SendCoreAsync("SDI3GOutputLevelChangedEvent", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeAutoVideoModeChanged:
                    _hub.Clients.All.SendCoreAsync("TypeAutoVideoModeChanged", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeAutoVideoModeDetectedChanged:
                    _hub.Clients.All.SendCoreAsync("AutoVideoModeDetectedChanged", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeSuperSourceCascadeChanged:
                    _hub.Clients.All.SendCoreAsync("SuperSourceCascadeChanged", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeTimeCodeChanged:
                    _hub.Clients.All.SendCoreAsync("TimeCodeChanged", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeTimeCodeLockedChanged:
                    _hub.Clients.All.SendCoreAsync("TimeCodeLockedChanged", null);
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeTimeCodeModeChanged:
                    _hub.Clients.All.SendCoreAsync("TimeCodeModeChanged", null);
                    break;
                default:
                    break;
            }
        }

        #region Switcher connect/disconnect and supporting methods
        private void SwitcherDisconnected()
        {
            if (_switcher != null)
            {
                // Remove callback:
                _switcher.RemoveCallback(this);

                // release reference:
                _switcher = null;
            }
        }

        private void SwitcherConnected()
        {
            // Get the switcher name:
            _switcher.GetProductName(out string switcherName);
            SwitcherName = switcherName;
        }

        public IBMDSwitcher Connect(string address)
        {
            this.AtemIPAddress = address;

            _BMDSwitcherConnectToFailure failReason = 0;

            try
            {
                // TODO: See notes on connection - move to a thread
                // Note that ConnectTo() can take several seconds to return, both for success or failure,
                // depending upon hostname resolution and network response times, so it may be best to
                // do this in a separate thread to prevent the main GUI thread blocking.
                _switcherDiscovery.ConnectTo(address, out _switcher, out failReason);
            }
            catch (COMException comEx)
            {
                // An exception will be thrown if ConnectTo fails. For more information, see failReason.
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

            SwitcherConnected();
            return _switcher;
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Atem() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }


        #endregion
    }
}