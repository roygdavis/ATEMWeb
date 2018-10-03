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
    class Atem
    {
        private IBMDSwitcherDiscovery m_switcherDiscovery;
        private IBMDSwitcher m_switcher;
        private IBMDSwitcherMixEffectBlock m_mixEffectBlock1;
        private IBMDSwitcherMixEffectBlock m_mixEffectBlock2;
        private IBMDSwitcherInputSuperSource m_superSource;

        private SwitcherMonitor m_switcherMonitor;
        private MixEffectBlockMonitor m_mixEffectBlockMonitor;

        private List<AtemInput> inputs;
        private List<KeyValuePair<string, IBMDSwitcherKey>> keys;
        private List<SuperSourceBox> supersourceBoxes;

        public Atem()
        {
            inputs = new List<AtemInput>();
            keys = new List<KeyValuePair<string, IBMDSwitcherKey>>();
            supersourceBoxes = new List<SuperSourceBox>();

            m_switcherMonitor = new SwitcherMonitor();
            // note: this invoke pattern ensures our callback is called in the main thread. We are making double
            // use of lambda expressions here to achieve this.
            // Essentially, the events will arrive at the callback class (implemented by our monitor classes)
            // on a separate thread. We must marshal these to the main thread, and we're doing this by calling
            // invoke on the Windows Forms object. The lambda expression is just a simplification.
            m_switcherMonitor.SwitcherDisconnected += new SwitcherEventHandler((s, a) => SwitcherDisconnected());

            m_mixEffectBlockMonitor = new MixEffectBlockMonitor();
            m_mixEffectBlockMonitor.ProgramInputChanged += new SwitcherEventHandler((s, a) => OnProgramInputChanged());
            m_mixEffectBlockMonitor.PreviewInputChanged += new SwitcherEventHandler((s, a) => OnProgramInputChanged());

            m_switcherDiscovery = new CBMDSwitcherDiscovery();
            if (m_switcherDiscovery == null)
            {
                // TODO: Raise exeception
            }

            SwitcherDisconnected();		// start with switcher disconnected
        }

        internal List<KeyValuePair<string, long>> GetSourcesKeyValuePairs()
        {
            List<KeyValuePair<string, long>> sources = new List<KeyValuePair<string, long>>();
            foreach (var item in this.inputs)
            {
                long inputid;
                item.BMDInput.GetInputId(out inputid);
                KeyValuePair<string, long> pair = new KeyValuePair<string, long>(item.AtemName, inputid);
                sources.Add(pair);
            }
            return sources;
        }

        public event EventHandler PGMInputChanged;
        public event EventHandler PVWInputChanged;

        internal void SetSuperSourceSource(long inputId)
        {
            this.supersourceBoxes[3].BMDSuperSourceBox.SetInputSource(inputId);
        }

        public event EventHandler Connected;
        public event EventHandler Disconnected;

        protected void OnProgramInputChanged()
        {
            if (this.PGMInputChanged != null)
            {
                PGMInputChanged(this, new EventArgs());
            }
        }

        protected void OnPreviewInputChanged()
        {
            if (this.PVWInputChanged != null)
            {
                PVWInputChanged(this, new EventArgs());
            }
        }

        private void SwitcherDisconnected()
        {
            if (m_switcher != null)
            {
                // Remove callback:
                m_switcher.RemoveCallback(m_switcherMonitor);

                // release reference:
                m_switcher = null;
            }

            if (m_mixEffectBlock1 != null)
            {
                // Remove callback
                m_mixEffectBlock1.RemoveCallback(m_mixEffectBlockMonitor);

                // Release reference
                m_mixEffectBlock1 = null;
            }
            if (m_mixEffectBlock2 != null)
            {
                // Remove callback
                m_mixEffectBlock2.RemoveCallback(m_mixEffectBlockMonitor);

                // Release reference
                m_mixEffectBlock2 = null;
            }
            if (this.Disconnected != null)
            {
                Disconnected(this, new EventArgs());
            }
        }

        private void SwitcherConnected()
        {
            // TODO: Make this an event

            // Get the switcher name:
            string switcherName;
            m_switcher.GetProductName(out switcherName);

            // Install SwitcherMonitor callbacks:
            m_switcher.AddCallback(m_switcherMonitor);

            // mix effects blocks
            getMEBlocks();

            // keyers
            getKeyers();

            // inputs
            getInputs();

            // super source
            getSupersourceBoxes();

            InitATEM();

            if (this.Connected != null)
            {
                Connected(this, new EventArgs());
            }
        }

        private void getMEBlocks()
        {
            // We want to get the first Mix Effect block (ME 1). We create a ME iterator,
            // and then get the first one:
            m_mixEffectBlock1 = null;
            m_mixEffectBlock2 = null;

            IBMDSwitcherMixEffectBlockIterator meIterator = null;
            IntPtr meIteratorPtr;
            Guid meIteratorIID = typeof(IBMDSwitcherMixEffectBlockIterator).GUID;
            m_switcher.CreateIterator(ref meIteratorIID, out meIteratorPtr);
            if (meIteratorPtr != null)
            {
                meIterator = (IBMDSwitcherMixEffectBlockIterator)Marshal.GetObjectForIUnknown(meIteratorPtr);
            }

            if (meIterator == null)
                return;

            if (meIterator != null)
            {
                meIterator.Next(out m_mixEffectBlock1);
                meIterator.Next(out m_mixEffectBlock2);
            }

            if (m_mixEffectBlock1 == null)
            {
                //MessageBox.Show("Unexpected: Could not get first mix effect block", "Error");
                return;
            }

            if (m_mixEffectBlock2 == null)
            {
                //MessageBox.Show("Unexpected: Could not get first mix effect block", "Error");
                return;
            }

            // Install MixEffectBlockMonitor callbacks:
            m_mixEffectBlock1.AddCallback(m_mixEffectBlockMonitor);
            m_mixEffectBlock2.AddCallback(m_mixEffectBlockMonitor);
        }

        private void getInputs()
        {
            bool iterating;
            IntPtr inputsPtr;
            Guid inputsGuid = typeof(IBMDSwitcherInputIterator).GUID;
            m_switcher.CreateIterator(ref inputsGuid, out inputsPtr);

            IBMDSwitcherInputIterator inputsIterator = (IBMDSwitcherInputIterator)Marshal.GetObjectForIUnknown(inputsPtr);

            iterating = true;
            while (iterating)
            {
                IBMDSwitcherInput input;
                inputsIterator.Next(out input);
                if (input != null)
                {
                    string name;
                    input.GetString(_BMDSwitcherInputPropertyId.bmdSwitcherInputPropertyIdLongName, out name);
                    inputs.Add(new AtemInput(name, ref input));
                    //inputs.Add(new AtemInput(SwitcherMappings.NameMappings.First(i => i.Key == name).Value, ref input));
                }
                else
                {
                    iterating = false;
                }
            }

            //// supersource
            //IBMDSwitcherInput ssInput;
            //long ssInt = 6000;
            //inputsIterator.GetById(ssInt, out ssInput);

            //IntPtr ssPtr;
            //GCHandle inputHandle = GCHandle.Alloc(ssInput);
            //IntPtr inputPtr = (IntPtr)inputHandle;
            //Guid ssGuid = typeof(IBMDSwitcherInputSuperSource).GUID;
            //int hresult = Marshal.QueryInterface(inputPtr, ref ssGuid, out ssPtr);
            //if (ssPtr != IntPtr.Zero)
            //{
            //    Console.Write("SUCCESS");
            //}
        }

        private void getKeyers()
        {
            bool iterating;
            int keyerNum;
            IntPtr keyersPtr;
            Guid keyerGuid = typeof(IBMDSwitcherKeyIterator).GUID;
            m_mixEffectBlock1.CreateIterator(ref keyerGuid, out keyersPtr);

            IBMDSwitcherKeyIterator keyersIterator = (IBMDSwitcherKeyIterator)Marshal.GetObjectForIUnknown(keyersPtr);
            iterating = true;
            keyerNum = 1;
            while (iterating)
            {
                IBMDSwitcherKey keyer;
                keyersIterator.Next(out keyer);
                if (keyer != null)
                {
                    keys.Add(new KeyValuePair<string, IBMDSwitcherKey>(string.Format("ME1KEY{0}", keyerNum), keyer));
                }
                else
                {
                    iterating = false;
                }
                keyerNum++;
            }

            keyersIterator = null;
            keyersPtr = IntPtr.Zero;

            m_mixEffectBlock2.CreateIterator(ref keyerGuid, out keyersPtr);

            keyersIterator = (IBMDSwitcherKeyIterator)Marshal.GetObjectForIUnknown(keyersPtr);
            iterating = true;
            keyerNum = 1;
            while (iterating)
            {
                IBMDSwitcherKey keyer;
                keyersIterator.Next(out keyer);
                if (keyer != null)
                {
                    keys.Add(new KeyValuePair<string, IBMDSwitcherKey>(string.Format("ME2KEY{0}", keyerNum), keyer));
                }
                else
                {
                    iterating = false;
                }
                keyerNum++;
            }
        }

        private void getSupersourceBoxes()
        {

            //Guid ssGuid = typeof(IBMDSwitcherInputSuperSource).GUID;





            //IBMDSwitcherInputSuperSource supersource = (IBMDSwitcherInputSuperSource)Marshal.GetObjectForIUnknown(ssPtr);
            IntPtr ssPtr;
            Guid ssGuid = typeof(IBMDSwitcherSuperSourceBoxIterator).GUID;
            m_switcher.CreateIterator(ref ssGuid, out ssPtr);
            IBMDSwitcherSuperSourceBoxIterator ssIterator = (IBMDSwitcherSuperSourceBoxIterator)Marshal.GetObjectForIUnknown(ssPtr);
            bool iterating = true;
            int boxNum = 1;
            while (iterating)
            {
                IBMDSwitcherSuperSourceBox ss;
                ssIterator.Next(out ss);
                if (ss != null)
                {
                    supersourceBoxes.Add(new SuperSourceBox(ref ss));
                }
                else
                {
                    iterating = false;
                }
                boxNum++;
            }

            ssIterator = null;
            ssPtr = IntPtr.Zero;

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

        public void InitATEM()
        {
            // ME2
            m_mixEffectBlock2.SetInt(_BMDSwitcherMixEffectBlockPropertyId.bmdSwitcherMixEffectBlockPropertyIdProgramInput, inputs.First(i => i.InputName == InputNames.Caspar1).AtemId);
            m_mixEffectBlock2.SetInt(_BMDSwitcherMixEffectBlockPropertyId.bmdSwitcherMixEffectBlockPropertyIdPreviewInput, inputs.First(i => i.InputName == InputNames.Caspar2).AtemId);
            keys.First(k => k.Key == "ME2KEY1").Value.SetOnAir(1);
            keys.First(k => k.Key == "ME2KEY2").Value.SetOnAir(0);

            // ME1
            keys.First(k => k.Key == "ME1KEY1").Value.SetOnAir(0);
            keys.First(k => k.Key == "ME1KEY2").Value.SetOnAir(0);
            m_mixEffectBlock1.SetInt(_BMDSwitcherMixEffectBlockPropertyId.bmdSwitcherMixEffectBlockPropertyIdProgramInput, inputs.First(i => i.InputName == InputNames.Caspar4).AtemId);
            m_mixEffectBlock1.SetInt(_BMDSwitcherMixEffectBlockPropertyId.bmdSwitcherMixEffectBlockPropertyIdProgramInput, inputs.First(i => i.InputName == InputNames.ME2PGM).AtemId);
        }

        public void SetPGM(long inputId)
        {
            if (m_mixEffectBlock1 != null)
            {
                m_mixEffectBlock1.SetInt(_BMDSwitcherMixEffectBlockPropertyId.bmdSwitcherMixEffectBlockPropertyIdProgramInput,
                    inputId);
            }
        }

        public void SetPVW(long inputId)
        {
            if (m_mixEffectBlock1 != null)
            {
                m_mixEffectBlock1.SetInt(_BMDSwitcherMixEffectBlockPropertyId.bmdSwitcherMixEffectBlockPropertyIdPreviewInput,
                    inputId);
            }
        }

        public void CUT()
        {
            if (m_mixEffectBlock1 != null)
            {
                m_mixEffectBlock1.PerformCut();
            }
        }

        public void AUTO()
        {
            if (m_mixEffectBlock1 != null)
            {
                m_mixEffectBlock1.PerformAutoTransition();
            }
        }

        public void Switch_Cam1()
        {
            m_mixEffectBlock2.PerformCut();
        }

        public void Switch_Cam2()
        {
            m_mixEffectBlock2.PerformCut();
        }

        public void Switch_Cam3()
        {

        }

        public void Switch_Cam4()
        {

        }

        public void Switch_VT()
        {
            m_mixEffectBlock1.SetString(_BMDSwitcherMixEffectBlockPropertyId.bmdSwitcherMixEffectBlockPropertyIdProgramInput, inputs.First(i => i.InputName == InputNames.Caspar4).AtemName);
        }

        public void Switch_FF()
        {
            m_mixEffectBlock1.SetString(_BMDSwitcherMixEffectBlockPropertyId.bmdSwitcherMixEffectBlockPropertyIdProgramInput, inputs.First(i => i.InputName == InputNames.Caspar3).AtemName);
        }

        public void Switch_SS()
        {
            m_mixEffectBlock1.SetString(_BMDSwitcherMixEffectBlockPropertyId.bmdSwitcherMixEffectBlockPropertyIdProgramInput, inputs.First(i => i.InputName == InputNames.SUPERSOURCE).AtemName);
        }

    }

    class MEState
    {
        public IBMDSwitcherMixEffectBlock MixEffectsBlock { get; set; }
        public long PVW { get; set; }
        public long PGM { get; set; }

        public MEState(ref IBMDSwitcherMixEffectBlock meBlock1)
        {
            this.MixEffectsBlock = meBlock1;

            long pvw;
            this.MixEffectsBlock.GetInt(_BMDSwitcherMixEffectBlockPropertyId.bmdSwitcherMixEffectBlockPropertyIdPreviewInput, out pvw);
            this.PVW = pvw;

            long pgm;
            this.MixEffectsBlock.GetInt(_BMDSwitcherMixEffectBlockPropertyId.bmdSwitcherMixEffectBlockPropertyIdProgramInput, out pgm);
            this.PGM = pgm;
        }
    }

    enum InputNames
    {
        Caspar1,
        Caspar2,
        Caspar3,
        Caspar4,
        ME2PGM,
        ME2PVW,
        ME1PGM,
        ME1PVW,
        SUPERSOURCE,
        MEDIAPLAYER1,
        MEDIAPLAYER2,
        SCANCONVERTOR,
        OTHER
    }

    class AtemInput
    {
        public InputNames InputName { get; set; }

        public IBMDSwitcherInput BMDInput { get; set; }
        public string AtemName
        {
            get
            {
                string name;
                this.BMDInput.GetString(_BMDSwitcherInputPropertyId.bmdSwitcherInputPropertyIdLongName, out name);
                return name;
            }
        }
        public long AtemId
        {
            get
            {
                long id;
                this.BMDInput.GetInputId(out id);
                return id;
            }
        }
        public AtemInput(InputNames name, ref IBMDSwitcherInput input)
        {
            this.BMDInput = input;
            this.InputName = name;
        }
        public AtemInput(string name, ref IBMDSwitcherInput input)
        {
            InputNames inName;
            try
            {
                inName = SwitcherMappings.NameMappings.First(i => i.Key == name).Value;
            }
            catch
            {
                inName = InputNames.OTHER;
            }
            this.InputName = inName;
            this.BMDInput = input;
        }
    }

    class SuperSourceBox
    {
        public IBMDSwitcherSuperSourceBox BMDSuperSourceBox { get; set; }
        public long GetBoxInputId
        {
            get
            {
                long id;
                this.BMDSuperSourceBox.GetInputSource(out id);
                return id;

            }
        }

        public SuperSourceBox(ref IBMDSwitcherSuperSourceBox ss)
        {
            this.BMDSuperSourceBox = ss;
        }
    }

    static class SwitcherMappings
    {
        public static List<KeyValuePair<string, InputNames>> NameMappings
        {
            get
            {
                return new List<KeyValuePair<string, InputNames>>()
                {
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_CASPAR1"],InputNames.Caspar1),
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_CASPAR2"],InputNames.Caspar2),
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_CASPAR3"],InputNames.Caspar3),
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_CASPAR4"],InputNames.Caspar4),
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_SUPERSOURCE"],InputNames.SUPERSOURCE),
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_MEDIAPLAYER1"],InputNames.MEDIAPLAYER1),
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_MEDIAPLAYER2"],InputNames.MEDIAPLAYER2),
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_SCANCONVERTOR"],InputNames.SCANCONVERTOR),
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_ME1PGM"],InputNames.ME1PGM),
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_ME1PVW"],InputNames.ME1PVW),
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_ME2PGM"],InputNames.ME2PGM),
                    new KeyValuePair<string, InputNames>(ConfigurationManager.AppSettings["ATEM_ME2PVW"],InputNames.ME2PVW)
                };
            }
        }
    }
}