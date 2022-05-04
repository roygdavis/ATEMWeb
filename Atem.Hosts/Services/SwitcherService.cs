using Atem.Hosts.Core;
using Atem.Hosts.Switcher;
using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Services
{
    public class SwitcherService : ISwitcherService
    {
        private readonly ISwitcherHost _switcherHost;
        private int _meContextId;

        public SwitcherService(ISwitcherHost switcherHost)
        {
            _switcherHost = switcherHost;
        }

        public async Task AllowStreamingToResume() => await _switcherHost.AllowStreamingToResume();
        public async Task Connect(string address) => await _switcherHost.Connect(address);
        public async Task Disconnect() => await Task.CompletedTask;
        public async Task<bool> DoesSupportAutoVideoMode() => await _switcherHost.DoesSupportAutoVideoMode();
        public async Task<bool> DoesSupportDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to) => await _switcherHost.DoesSupportDownConvertedHDVideoMode(from, to);
        public async Task<bool> DoesSupportMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to) => await _switcherHost.DoesSupportMultiViewVideoMode(from, to);
        public async Task<bool> DoesSupportVideoMode(_BMDSwitcherVideoMode mode) => await _switcherHost.DoesSupportVideoMode(mode);
        public async Task<bool> DoesVideoModeChangeRequireReconfiguration(_BMDSwitcherVideoMode mode) => await _switcherHost.DoesVideoModeChangeRequireReconfiguration(mode);
        public async Task<_BMDSwitcher3GSDIOutputLevel> Get3GSDIOutputLevel() => await _switcherHost.Get3GSDIOutputLevel();
        public async Task<bool> GetAreOutputsConfigurable() => await _switcherHost.GetAreOutputsConfigurable();
        public async Task<bool> GetAutoVideoMode() => await _switcherHost.GetAutoVideoMode();
        public async Task<bool> GetAutoVideoModeDetected() => await _switcherHost.GetAutoVideoModeDetected();
        public async Task<_BMDSwitcherVideoMode> GetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from) => await _switcherHost.GetDownConvertedHDVideoMode(from);
        public async Task<uint> GetFadeToBlackFramesRemaining() => await _switcherHost.MixEffects[_meContextId].GetFadeToBlackFramesRemaining();
        public async Task<int> GetFadeToBlackFullyBlack() => await _switcherHost.MixEffects[_meContextId].GetFadeToBlackFullyBlack();
        public async Task<int> GetFadeToBlackInTransition() => await _switcherHost.MixEffects[_meContextId].GetFadeToBlackInTransition();
        public async Task<uint> GetFadeToBlackRate() => await _switcherHost.MixEffects[_meContextId].GetFadeToBlackRate();
        public async Task<int> GetInFadeToBlack() => await _switcherHost.MixEffects[_meContextId].GetInFadeToBlack();
        public async Task<_BMDSwitcherInputAvailability> GetInputAvailabilityMask() => await _switcherHost.MixEffects[_meContextId].GetInputAvailabilityMask();
        public async Task<int> GetInTransition() => await _switcherHost.MixEffects[_meContextId].GetInTransition();
        public async Task<_BMDSwitcherDownConversionMethod> GetMethodForDownConvertedSD() => await _switcherHost.GetMethodForDownConvertedSD();
        public async Task<_BMDSwitcherVideoMode> GetMultiViewVideoMode(_BMDSwitcherVideoMode from) => await _switcherHost.GetMultiViewVideoMode(from);
        public async Task<_BMDSwitcherPowerStatus> GetPowerStatus() => await _switcherHost.GetPowerStatus();
        public async Task<long> GetPreviewInput() => await _switcherHost.MixEffects[_meContextId].GetPreviewInput();
        public async Task<int> GetPreviewLive() => await _switcherHost.MixEffects[_meContextId].GetPreviewLive();
        public async Task<int> GetPreviewTransition() => await _switcherHost.MixEffects[_meContextId].GetPreviewTransition();
        public async Task<string> GetProductName() => await _switcherHost.GetProductName();
        public async Task<long> GetProgramInput() => await _switcherHost.MixEffects[_meContextId].GetProgramInput();
        public async Task<bool> GetSuperSourceCascade() => await _switcherHost.GetSuperSourceCascade();
        public async Task<TimeCode> GetTimeCode() => await _switcherHost.GetTimeCode();
        public async Task<bool> GetTimeCodeLocked() => await _switcherHost.GetTimeCodeLocked();
        public async Task<_BMDSwitcherTimeCodeMode> GetTimeCodeMode() => await _switcherHost.GetTimeCodeMode();
        public async Task<uint> GetTransitionFramesRemaining() => await _switcherHost.MixEffects[_meContextId].GetTransitionFramesRemaining();
        public async Task<double> GetTransitionPosition() => await _switcherHost.MixEffects[_meContextId].GetTransitionPosition();
        public async Task<_BMDSwitcherVideoMode> GetVideoMode() => await _switcherHost.GetVideoMode();
        public async Task PerformAutoTransition() => await _switcherHost.MixEffects[_meContextId].PerformAutoTransition();
        public async Task PerformCut() => await _switcherHost.MixEffects[_meContextId].PerformCut();
        public async Task PerformFadeToBlack() => await _switcherHost.MixEffects[_meContextId].PerformFadeToBlack();
        public async Task RequestTimeCode() => await _switcherHost.RequestTimeCode();
        public async Task Set3GSDIOutputLevel(_BMDSwitcher3GSDIOutputLevel level) => await _switcherHost.Set3GSDIOutputLevel(level);
        public async Task SetAutoVideoMode(bool enabled) => await _switcherHost.SetAutoVideoMode(enabled);
        public async Task SetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to) => await _switcherHost.SetDownConvertedHDVideoMode(from, to);
        public async Task SetFadeToBlackFullyBlack(int value) => await _switcherHost.MixEffects[_meContextId].SetFadeToBlackFullyBlack(value);
        public async Task SetFadeToBlackRate(uint value) => await _switcherHost.MixEffects[_meContextId].SetFadeToBlackRate(value);
        public async Task SetMethodForDownConvertedSD(_BMDSwitcherDownConversionMethod method) => await _switcherHost.SetMethodForDownConvertedSD(method);
        public async Task SetMixEffectsContext(int mixEffectIndex) => await Task.Run(() => { _meContextId = mixEffectIndex; });
        public async Task SetMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to) => await _switcherHost.SetMultiViewVideoMode(from, to);
        public async Task SetPreviewInput(long value) => await _switcherHost.MixEffects[_meContextId].SetPreviewInput(value);
        public async Task SetPreviewTransition(int value) => await _switcherHost.MixEffects[_meContextId].SetPreviewTransition(value);
        public async Task SetProgramInput(long value) => await _switcherHost.MixEffects[_meContextId].SetProgramInput(value);
        public async Task SetSuperSourceCascade(int cascade) => await _switcherHost.SetSuperSourceCascade(cascade);
        public async Task SetTimeCode(TimeCode timeCode) => await _switcherHost.SetTimeCode(timeCode);
        public async Task SetTimeCodeMode(_BMDSwitcherTimeCodeMode mode) => await _switcherHost.SetTimeCodeMode(mode);
        public async Task SetTransitionPosition(double value) => await _switcherHost.MixEffects[_meContextId].SetTransitionPosition(value);
        public async Task SetVideoMode(_BMDSwitcherVideoMode mode) => await _switcherHost.SetVideoMode(mode);
    }
}