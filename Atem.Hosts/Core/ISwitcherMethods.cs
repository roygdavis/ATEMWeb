using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Core
{
    public interface ISwitcherMethods
    {
        Task Connect(string address);
        Task AllowStreamingToResume();
        Task<bool> DoesSupportAutoVideoMode();
        Task<bool> DoesSupportDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to);
        Task<bool> DoesSupportMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to);
        Task<bool> DoesSupportVideoMode(_BMDSwitcherVideoMode mode);
        Task<bool> DoesVideoModeChangeRequireReconfiguration(_BMDSwitcherVideoMode mode);
        Task<_BMDSwitcher3GSDIOutputLevel> Get3GSDIOutputLevel();
        Task<string> GetProductName();
        Task<_BMDSwitcherVideoMode> GetVideoMode();
        Task<_BMDSwitcherDownConversionMethod> GetMethodForDownConvertedSD();
        Task<_BMDSwitcherVideoMode> GetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from);
        Task<_BMDSwitcherVideoMode> GetMultiViewVideoMode(_BMDSwitcherVideoMode from);
        Task<_BMDSwitcherPowerStatus> GetPowerStatus();
        Task<TimeCode> GetTimeCode();
        Task<bool> GetTimeCodeLocked();
        Task<_BMDSwitcherTimeCodeMode> GetTimeCodeMode();
        Task<bool> GetAreOutputsConfigurable();
        Task<bool> GetSuperSourceCascade();
        Task<bool> GetAutoVideoMode();
        Task<bool> GetAutoVideoModeDetected();
        Task SetMethodForDownConvertedSD(_BMDSwitcherDownConversionMethod method);
        Task SetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to);
        Task SetMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to);
        Task Set3GSDIOutputLevel(_BMDSwitcher3GSDIOutputLevel level);
        Task SetTimeCode(TimeCode timeCode);
        Task RequestTimeCode();
        Task SetTimeCodeMode(_BMDSwitcherTimeCodeMode mode);
        Task SetSuperSourceCascade(int cascade);
        Task SetAutoVideoMode(bool enabled);
        Task SetVideoMode(_BMDSwitcherVideoMode mode);
    }
}
