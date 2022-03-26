using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Runtime.Serialization;
using BMDSwitcherAPI;
using Atem.Hosts.Legacy.Interfaces;
using Atem.Hosts.Legacy.Exceptions;
using Atem.Hosts.Legacy.Hosts.MixEffects;
using Atem.Hosts.Legacy.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Atem.Hosts.Legacy.Hosts
{
    [DataContract]
    public class SwitcherHost : ISwitcherHost, IDisposable, IBMDSwitcherCallback,ISwitcherHandlers<EventArgs>
    {
        #region private fields
        /// <summary>
        /// Holds the switcher enumerator object
        /// </summary>
        private readonly IBMDSwitcherDiscovery _switcherDiscovery;
        private readonly IHubContext<ATEMEventsHub> _hub;

        /// <summary>
        /// Holds the switcher object
        /// </summary>
        private IBMDSwitcher _switcher;
        #endregion

        #region Public properties

        
        public _BMDSwitcherDownConversionMethod MethodForDownConvertedSD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public _BMDSwitcherDownConversionMethod DownConvertedHDVideoMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public _BMDSwitcherVideoMode MultiViewVideoMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Disconnected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long TimeCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TimeCodeLocked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public _BMDSwitcherTimeCodeMode TimeCodeMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool SuperSourceCascade { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoVideoMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoVideoModeDetected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public _BMDSwitcherVideoMode VideoMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public _BMDSwitcherPowerStatus PowerStatus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public _BMDSwitcher3GSDIOutputLevel SDI3GOutputLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AtemIPAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ProductName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion

        #region Constructors
        public SwitcherHost(IHubContext<ATEMEventsHub> hub)
        {
            _switcherDiscovery = new CBMDSwitcherDiscovery();
            if (_switcherDiscovery == null)
            {
                // TODO: Raise exeception
            }
            _hub = hub;
            SwitcherDisconnected();		// start with switcher disconnected
        }
        #endregion

        public void Notify(_BMDSwitcherEventType eventType, _BMDSwitcherVideoMode coreVideoMode)
        {
            // TODO: Each event should provide the value from the _switcher.GetXXX method
            //switch (eventType)
            //{
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypeVideoModeChanged:
            //        VideoModeChangedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypeMethodForDownConvertedSDChanged:
            //        MethodForDownConvertedSDChangedEvent?.Invoke(this, new EventArgs()); ;
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypeDownConvertedHDVideoModeChanged:
            //        DownConvertedHDVideoModeChangedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypeMultiViewVideoModeChanged:
            //        MultiViewVideoModeChangedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypePowerStatusChanged:
            //        PowerStatusChangedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypeDisconnected:
            //        DisconnectedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventType3GSDIOutputLevelChanged:
            //        SDI3GOutputLevelChangedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypeAutoVideoModeChanged:
            //        TypeAutoVideoModeChangedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypeAutoVideoModeDetectedChanged:
            //        AutoVideoModeDetectedChangedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypeSuperSourceCascadeChanged:
            //        SuperSourceCascadeChangedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypeTimeCodeChanged:
            //        TimeCodeChangedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypeTimeCodeLockedChanged:
            //        TimeCodeLockedChangedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    case _BMDSwitcherEventType.bmdSwitcherEventTypeTimeCodeModeChanged:
            //        TimeCodeModeChangedEvent?.Invoke(this, new EventArgs());
            //        break;
            //    default:
            //        break;
            //}
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
            //SwitcherName = switcherName;
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