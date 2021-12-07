using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixteenMedia.ATEM.Broker.BMDSwitcherAPI;

namespace SixteenMedia.ATEM.Wrapper
{
    public class Input : IBMDSwitcherInputCallback
    {
        private IBMDSwitcherInput m_switcherInput;

        public _BMDSwitcherPortType PortType
        {
            get
            {
                m_switcherInput.GetPortType(out _BMDSwitcherPortType type);
                return type;
            }
        }

        public _BMDSwitcherInputAvailability InputAvailability
        {
            get
            {
                m_switcherInput.GetInputAvailability(out _BMDSwitcherInputAvailability availability);
                return availability;
            }
        }

        public string ShortName
        {
            get
            {
                m_switcherInput.GetShortName(out string name);
                return name;
            }
            set
            {
                m_switcherInput.SetShortName(value);
            }
        }

        public string LongName
        {
            get
            {
                m_switcherInput.GetLongName(out string name);
                return name;
            }
            set
            {
                m_switcherInput.SetLongName(value);
            }
        }

        public int AreNamesDefault
        {
            get
            {
                int isDefault = 0;
                m_switcherInput.AreNamesDefault(ref isDefault);
                return isDefault;
            }
        }

        public int IsProgramTallied
        {
            get
            {
                int isTallied = 0;
                m_switcherInput.IsProgramTallied(out isTallied);
                return isTallied;
            }
        }

        public int IsPreviewTallied
        {
            get
            {
                int isTallied = 0;
                m_switcherInput.IsPreviewTallied(out isTallied);
                return isTallied;
            }
        }

        void ResetNames();
        
        void GetAvailableExternalPortTypes(out _BMDSwitcherExternalPortType types);
        void SetCurrentExternalPortType(_BMDSwitcherExternalPortType value);
        void GetCurrentExternalPortType(out _BMDSwitcherExternalPortType value);
        void GetInputId(out long inputId);
        void AddCallback(IBMDSwitcherInputCallback callback);
        void RemoveCallback(IBMDSwitcherInputCallback callback);

        public void Notify(_BMDSwitcherInputEventType eventType)
        {
            switch (eventType)
            {
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeShortNameChanged:
                    break;
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeLongNameChanged:
                    break;
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeAreNamesDefaultChanged:
                    break;
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeIsProgramTalliedChanged:
                    break;
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeIsPreviewTalliedChanged:
                    break;
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeAvailableExternalPortTypesChanged:
                    break;
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeCurrentExternalPortTypeChanged:
                    break;
                default:
                    break;
            }
        }
    }
}
