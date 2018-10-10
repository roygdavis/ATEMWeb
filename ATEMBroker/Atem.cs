using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixteenMedia.ATEM.Broker.BMDSwitcherAPI;
using System.Runtime.InteropServices;
using System.Configuration;

namespace SixteenMedia.ATEM.Broker
{
    public class Atem:IBMDSwitcherCallback,
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
        private Extensions.MEBlock[] m_mixEffectBlocks;
        #endregion

        #region Events
        
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
        /// Called when the PGM bus input changes successfully
        /// </summary>
        public event EventHandler<MixEffectsEventArgs> PGMInputChanged;
        
        /// <summary>
        /// Called when the PVW bus input changes successfully
        /// </summary>
        public event EventHandler<MixEffectsEventArgs> PVWInputChanged;

        /// <summary>
        /// Called when the Atem is connected successfully
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// Called when the Atem is disconnected successfully
        /// </summary>
        public event EventHandler Disconnected;

        protected void OnProgramInputChanged(MixEffectsEventArgs e)
        {
            if (this.PGMInputChanged != null)
            {
                PGMInputChanged(this, e);
            }
        }

        protected void OnPreviewInputChanged(MixEffectsEventArgs e)
        {
            if (this.PVWInputChanged != null)
            {
                PVWInputChanged(this, e);
            }
        }

        public void Notify(_BMDSwitcherEventType eventType, _BMDSwitcherVideoMode coreVideoMode)
        {
            throw new NotImplementedException();
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
            if (this.Disconnected != null)
            {
                Disconnected(this, new EventArgs());
            }
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

        private void getMEBlocks()
        {
            // ensure m_mixEffectBlocks is empty
            nullifyMixEffectsBlocks();

            // Create an iterator
            IBMDSwitcherMixEffectBlockIterator meIterator = null;

            // init the mix effects count, this is a zero-based count so we start at -1
            int meCount = -1;

            // Holds the pointer to the mix effects iterator object
            IntPtr meIteratorPtr;

            // The COM class GUID of the iterator
            Guid meIteratorIID = typeof(IBMDSwitcherMixEffectBlockIterator).GUID;

            // Now create the iterator
            m_switcher.CreateIterator(ref meIteratorIID, out meIteratorPtr);

            // if we're not null then do the conversion to a .net class
            if (meIteratorPtr != null)
            {
                meIterator = (IBMDSwitcherMixEffectBlockIterator)Marshal.GetObjectForIUnknown(meIteratorPtr);
            }

            // bail if that returned null
            if (meIterator == null)
                return;

            // now we can start to iterate over the ME blocks this ATEM has. Usually the basic ATEM's have only one Mix Effect Block.  The 2M/E ATEM's have two.  Some have more.
            IBMDSwitcherMixEffectBlock meBlock = null;

            // try and get the Mix Effect Block from the iterator
            meIterator.Next(out meBlock);

            // if that wasn't null then add to our array of ME Blocks
            while (meBlock != null)
            {
                // We're not null, we increment our Mix Effect count!
                meCount++;

                // is this the first ME Block?
                if (meCount == 0)
                {
                    // Yes, so we init our ME Block array with just one item
                    m_mixEffectBlocks = new Extensions.MEBlock[1];
                    m_mixEffectBlocks[0] = new Extensions.MEBlock(meBlock, meCount);
                    m_mixEffectBlocks[0].ProgramInputChanged += Atem_ProgramInputChanged;
                }
                else // otherwise we re-create our ME Block array and increase size by one!
                {
                    Extensions.MEBlock[] oldMEArray = m_mixEffectBlocks;
                    m_mixEffectBlocks = new Extensions.MEBlock[meCount];
                    for (int i = 0; i < meCount; i++)
                    {
                        m_mixEffectBlocks[i] = oldMEArray[i];
                    }
                    m_mixEffectBlocks[meCount] = new Extensions.MEBlock(meBlock, meCount);
                }

                // Try and get the next block.  A ref of null means there are no more Mix Effects Blocks on this ATEM
                meIterator.Next(out meBlock);
            }
        }

        private void Atem_ProgramInputChanged(object sender, MixEffectsEventArgs e)
        {
            OnProgramInputChanged(e);
        }

        private void SwitcherConnected()
        {
            // TODO: Make this an event

            // Get the switcher name:
            string switcherName;
            m_switcher.GetProductName(out switcherName);

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
            _BMDSwitcherConnectToFailure failReason = 0;

            try
            {
                // Note that ConnectTo() can take several seconds to return, both for success or failure,
                // depending upon hostname resolution and network response times, so it may be best to
                // do this in a separate thread to prevent the main GUI thread blocking.
                m_switcherDiscovery.ConnectTo(address, out m_switcher, out failReason);
            }
            catch (COMException)
            {
                // An exception will be thrown if ConnectTo fails. For more information, see failReason.
                switch (failReason)
                {
                    case _BMDSwitcherConnectToFailure.bmdSwitcherConnectToFailureNoResponse:
                        // MessageBox.Show("No response from Switcher", "Error");
                        break;
                    case _BMDSwitcherConnectToFailure.bmdSwitcherConnectToFailureIncompatibleFirmware:
                        // MessageBox.Show("Switcher has incompatible firmware", "Error");
                        break;
                    default:
                        // MessageBox.Show("Connection failed for unknown reason", "Error");
                        break;
                }
                return;
            }

            SwitcherConnected();
        }
        #endregion

        /// <summary>
        /// Sets the input for the PGM bus on the ATEM
        /// </summary>
        /// <param name="inputId"></param>
        public void SetPGM(long inputId)
        {
            if (m_mixEffectBlocks != null)
            {
                for (int i = 0; i < m_mixEffectBlocks.Length; i++)
                {
                    m_mixEffectBlocks[i].ProgramInput = inputId;
                }
            }
        }

        /// <summary>
        /// Sets the input for PVW bus on the ATEM
        /// </summary>
        /// <param name="inputId"></param>
        public void SetPVW(long inputId)
        {
            if (m_mixEffectBlocks != null)
            {
                for (int i = 0; i < m_mixEffectBlocks.Length; i++)
                {
                    m_mixEffectBlocks[i].PreviewInput = inputId;
                }
            }
        }

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
        }

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
        }

        #endregion

        

    }
}