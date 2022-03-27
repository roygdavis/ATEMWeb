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

        public SwitcherService(ISwitcherHost switcherHost)
        {
            _switcherHost = switcherHost;
        }

        public async Task AllowStreamingToResume()
        {
            await _switcherHost.AllowStreamingToResume();
        }

        public async Task Connect(string address)
        {
            await _switcherHost.Connect(address);
        }

        public async Task Disconnect()
        {
            await Task.CompletedTask;
        }

        public async Task<bool> DoesSupportAutoVideoMode()
        {
            return await _switcherHost.DoesSupportAutoVideoMode();
        }

        public async Task<bool> DoesSupportDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            return await _switcherHost.DoesSupportDownConvertedHDVideoMode(from, to);
        }

        public async Task<bool> DoesSupportMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            return await _switcherHost.DoesSupportMultiViewVideoMode(from, to);
        }

        public async Task<bool> DoesSupportVideoMode(_BMDSwitcherVideoMode mode)
        {
            return await _switcherHost.DoesSupportVideoMode(mode);
        }

        public async Task<bool> DoesVideoModeChangeRequireReconfiguration(_BMDSwitcherVideoMode mode)
        {
            return await _switcherHost.DoesVideoModeChangeRequireReconfiguration(mode);
        }

        public async Task<_BMDSwitcher3GSDIOutputLevel> Get3GSDIOutputLevel()
        {
            return await _switcherHost.Get3GSDIOutputLevel();
        }

        public async Task<bool> GetAreOutputsConfigurable()
        {
            return await _switcherHost.GetAreOutputsConfigurable();
        }

        public async Task<bool> GetAutoVideoMode()
        {
            return await _switcherHost.GetAutoVideoMode();
        }

        public async Task<bool> GetAutoVideoModeDetected()
        {
            return await _switcherHost.GetAutoVideoModeDetected();
        }

        public async Task<_BMDSwitcherVideoMode> GetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from)
        {
            return await _switcherHost.GetDownConvertedHDVideoMode(from);
        }

        public async Task<_BMDSwitcherDownConversionMethod> GetMethodForDownConvertedSD()
        {
            return await _switcherHost.GetMethodForDownConvertedSD();
        }

        public async Task<_BMDSwitcherVideoMode> GetMultiViewVideoMode(_BMDSwitcherVideoMode from)
        {
            return await _switcherHost.GetMultiViewVideoMode(from);
        }

        public async Task<_BMDSwitcherPowerStatus> GetPowerStatus()
        {
            return await _switcherHost.GetPowerStatus();
        }

        public async Task<string> GetProductName()
        {
            return await _switcherHost.GetProductName();
        }

        public async Task<bool> GetSuperSourceCascade()
        {
            return await _switcherHost.GetSuperSourceCascade();
        }

        public async Task<TimeCode> GetTimeCode()
        {
            return await _switcherHost.GetTimeCode();
        }

        public async Task<bool> GetTimeCodeLocked()
        {
            return await _switcherHost.GetTimeCodeLocked();
        }

        public async Task<_BMDSwitcherTimeCodeMode> GetTimeCodeMode()
        {
            return await _switcherHost.GetTimeCodeMode();
        }

        public async Task<_BMDSwitcherVideoMode> GetVideoMode()
        {
            return await _switcherHost.GetVideoMode();
        }

        public async Task RequestTimeCode()
        {
            await _switcherHost.RequestTimeCode();
        }

        public async Task Set3GSDIOutputLevel(_BMDSwitcher3GSDIOutputLevel level)
        {
            await _switcherHost.Set3GSDIOutputLevel(level);
        }

        public async Task SetAutoVideoMode(bool enabled)
        {
            await _switcherHost.SetAutoVideoMode(enabled);
        }

        public async Task SetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            await _switcherHost.SetDownConvertedHDVideoMode(from, to);
        }

        public async Task SetMethodForDownConvertedSD(_BMDSwitcherDownConversionMethod method)
        {
            await _switcherHost.SetMethodForDownConvertedSD(method);
        }

        public async Task SetMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            await _switcherHost.SetMultiViewVideoMode(from, to);
        }

        public async Task SetPGM(int switcherIndex, long input)
        {
           await _switcherHost.MixEffects[switcherIndex].SetPGM(input);
        }

        public async Task SetPVW(int switcherIndex, long input)
        {
            await _switcherHost.MixEffects[switcherIndex].SetPVW(input);
        }

        public async Task SetSuperSourceCascade(int cascade)
        {
            await _switcherHost.SetSuperSourceCascade(cascade);
        }

        public async Task SetTimeCode(TimeCode timeCode)
        {
            await _switcherHost.SetTimeCode(timeCode);
        }

        public async Task SetTimeCodeMode(_BMDSwitcherTimeCodeMode mode)
        {
            await _switcherHost.SetTimeCodeMode(mode);
        }

        public async Task SetVideoMode(_BMDSwitcherVideoMode mode)
        {
            await _switcherHost.SetVideoMode(mode);
        }
    }
}