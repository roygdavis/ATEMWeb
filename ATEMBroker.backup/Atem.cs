﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixteenMedia.ATEM.Broker.BMDSwitcherAPI;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Runtime.Serialization;

namespace SixteenMedia.ATEM.Wrapper
{
    [DataContract]
    public class Atem : IBMDSwitcherCallback,
        IDisposable
    {
        #region private fields
        /// <summary>
        /// Holds the switcher enumerator object
        /// </summary>
        private IBMDSwitcherDiscovery m_switcherDiscovery;

        /// <summary>
        /// Holds the switcher object
        /// </summary>
        private IBMDSwitcher m_switcher;

        /// <summary>
        /// An array containing the ME Block objects
        /// </summary>
        private MEBlock[] m_mixEffectBlocks;
        #endregion

        #region Public properties
        [DataMember]
        public List<MEBlock> MixEffectsBlocks
        {
            get
            {
                if (m_mixEffectBlocks != null)
                {
                    return m_mixEffectBlocks.ToList();
                }
                else
                {
                    return new List<MEBlock>();
                }
            }
        }

        [DataMember]
        public string AtemIPAddress { get; set; }

        public static Atem Null => new Atem();
        #endregion

        #region Constructors
        public Atem()
        {
            
            //m_mixEffectBlockMonitor = new MixEffectBlockMonitor();
            //m_mixEffectBlockMonitor.ProgramInputChanged += new SwitcherEventHandler((s, a) => OnProgramInputChanged());
            //m_mixEffectBlockMonitor.PreviewInputChanged += new SwitcherEventHandler((s, a) => OnProgramInputChanged());

            m_switcherDiscovery = new CBMDSwitcherDiscovery();
            if (m_switcherDiscovery == null)
            {
                // TODO: Raise exeception
            }

            SwitcherDisconnected();		// start with switcher disconnected
        }
        #endregion

        #region Events

        /// <summary>
        /// Fired when a new Mix Effects block as detected and added
        /// </summary>
        public event EventHandler<MixEffectBlockConnectedEventArgs> MixEffectBlockConnectedEvent;
        
        /// <summary>
        /// Called when the Atem is connected successfully
        /// </summary>
        public event EventHandler<EventArgs> Connected;

        /// <summary>
        /// Called when the Atem is disconnected successfully
        /// </summary>
        public event EventHandler<EventArgs> DisconnectedEvent;

        public event EventHandler<EventArgs> VideoModeChangedEvent;
        public event EventHandler<EventArgs> MethodForDownConvertedSDChangedEvent;
        public event EventHandler<EventArgs> DownConvertedHDVideoModeChangedEvent;
        public event EventHandler<EventArgs> MultiViewVideoModeChangedEvent;
        public event EventHandler<EventArgs> PowerStatusChangedEvent;
        
        public event EventHandler<EventArgs> SDI3GOutputLevelChangedEvent;

        public void Notify(_BMDSwitcherEventType eventType, _BMDSwitcherVideoMode coreVideoMode)
        {
            switch (eventType)
            {
                case _BMDSwitcherEventType.bmdSwitcherEventTypeVideoModeChanged:
                    VideoModeChangedEvent?.Invoke(this, new EventArgs());
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeMethodForDownConvertedSDChanged:
                    MethodForDownConvertedSDChangedEvent?.Invoke(this, new EventArgs());
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeDownConvertedHDVideoModeChanged:
                    DownConvertedHDVideoModeChangedEvent?.Invoke(this, new EventArgs());
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeMultiViewVideoModeChanged:
                    MultiViewVideoModeChangedEvent?.Invoke(this, new EventArgs());
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypePowerStatusChanged:
                    PowerStatusChangedEvent?.Invoke(this, new EventArgs());
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeDisconnected:
                    DisconnectedEvent?.Invoke(this, new EventArgs());
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventType3GSDIOutputLevelChanged:
                    SDI3GOutputLevelChangedEvent?.Invoke(this, new EventArgs());
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Switcher connect/disconnect and supporting methods
        private void SwitcherDisconnected()
        {
            if (m_switcher != null)
            {
                // Remove callback:
                m_switcher.RemoveCallback(this);

                // release reference:
                m_switcher = null;
            }

            nullifyMixEffectsBlocks();
            DisconnectedEvent?.Invoke(this, new EventArgs());
        }

        private void nullifyMixEffectsBlocks()
        {
            if (m_mixEffectBlocks != null)
            {
                for (int i = 0; i < m_mixEffectBlocks.Length; i++)
                {
                    if (m_mixEffectBlocks[i] != null)
                    {
                        // Dispose reference
                        m_mixEffectBlocks[i].Dispose();
                    }
                }

                m_mixEffectBlocks = null;
            }
        }

        /// <summary>
        /// Creates the BMD iterator and retrives the Mix Effect(s) banks from the ATEM
        /// </summary>
        private void getMEBlocks()
        {
            // ensure m_mixEffectBlocks is empty
            nullifyMixEffectsBlocks();

            // init the mix effects count, this is a zero-based count so we start at -1
            int meCount = -1;

            // meIteratorIID holds the COM class GUID of the iterator
            Guid meIteratorIID = typeof(IBMDSwitcherMixEffectBlockIterator).GUID;

            // meIteratorPtr holds the pointer to the mix effects iterator object
            // create the iterator and out to the meIteratorPtr pointer
            m_switcher.CreateIterator(ref meIteratorIID, out IntPtr meIteratorPtr);

            // create the iterator
            IBMDSwitcherMixEffectBlockIterator meIterator = (IBMDSwitcherMixEffectBlockIterator)Marshal.GetObjectForIUnknown(meIteratorPtr);
            
            // bail if that returned null
            if (meIterator == null)
                return;

            // now we can start to iterate over the ME blocks this ATEM has. Usually the basic ATEM's have only one Mix Effect Block.  The 2M/E ATEM's have two.  Some have more.
            // try and get the Mix Effect Block from the iterator
            meIterator.Next(out IBMDSwitcherMixEffectBlock meBlock);

            // if that wasn't null then add to our array of ME Blocks
            while (meBlock != null)
            {
                // We're not null, we increment our Mix Effect count!
                meCount++;

                // is this the first ME Block?
                if (meCount == 0)
                {
                    // Yes, so we init our ME Block array with just one item
                    m_mixEffectBlocks = new MEBlock[1];
                    m_mixEffectBlocks[0] = new MEBlock(meBlock, meCount);
                }
                else // otherwise we re-create our ME Block array and increase size by one!
                {
                    MEBlock[] oldMEArray = m_mixEffectBlocks;
                    m_mixEffectBlocks = new MEBlock[meCount];
                    oldMEArray.CopyTo(m_mixEffectBlocks, 0);
                    m_mixEffectBlocks[meCount] = new MEBlock(meBlock, meCount);
                }

                // raise an event
                // the consumer of this event should hook into the mbBlock events
                MixEffectBlockConnectedEvent?.Invoke(this, new MixEffectBlockConnectedEventArgs(m_mixEffectBlocks[meCount]));

                // Try and get the next block.  A ref of null means there are no more Mix Effects Blocks on this ATEM
                meIterator.Next(out meBlock);
            }
        }

        private void SwitcherConnected()
        {
            // TODO: Make this an event

            // Get the switcher name:
            m_switcher.GetProductName(out string switcherName);

            // Install SwitcherMonitor callbacks:
            m_switcher.AddCallback(this);

            // mix effects blocks
            getMEBlocks();

            if (this.Connected != null)
            {
                Connected(this, new EventArgs());
            }
        }

        public void Connect(string address)
        {
            this.AtemIPAddress = address;

            _BMDSwitcherConnectToFailure failReason = 0;

            try
            {
                // Note that ConnectTo() can take several seconds to return, both for success or failure,
                // depending upon hostname resolution and network response times, so it may be best to
                // do this in a separate thread to prevent the main GUI thread blocking.
                m_switcherDiscovery.ConnectTo(address, out m_switcher, out failReason);
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
                    nullifyMixEffectsBlocks();
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

        #region IBMDSwitcher helper Getter/Setters
        [DataMember]
        public _BMDSwitcherVideoMode VideoMode
        {
            get
            {
                if (m_switcher != null)
                {
                    m_switcher.GetVideoMode(out _BMDSwitcherVideoMode v);
                    return v;
                }
                throw new NullReferenceException("m_switcher is null");
            }
            set
            {
                m_switcher.SetVideoMode(value);
            }
        }

        [DataMember]
        public string ProductName
        {
            get
            {
                if (m_switcher != null)
                {
                    m_switcher.GetProductName(out string v);
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
                if (m_switcher!=null)
                {
                    m_switcher.Get3GSDIOutputLevel(out _BMDSwitcher3GSDIOutputLevel v);
                    return v;
                }
                throw new NullReferenceException("m_switcher is null");
            }
            set
            {
                m_switcher.Set3GSDIOutputLevel(value);
            }
        }

        [DataMember]
        public _BMDSwitcherPowerStatus PowerStatus
        {
            get
            {
                if (m_switcher != null)
                {
                    m_switcher.GetPowerStatus(out _BMDSwitcherPowerStatus v);
                    return v;
                }
                throw new NullReferenceException("m_switcher is null");
            }
            // empty setter so that this property can be serialised
            set { }
        }

        #endregion
        
    }
}