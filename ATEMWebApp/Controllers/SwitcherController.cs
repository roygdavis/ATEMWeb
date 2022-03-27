using Atem.Hosts.Core;
using Atem.Hosts.Exceptions;
using Atem.Hosts.Services;
using Atem.Hosts.Switcher;
using BMDSwitcherAPI;
//using ATEM.Services;
//using ATEM.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATEMWebApp.Controllers
{
    [ApiController]
    [Route("api")]
    public class SwitcherController : ControllerBase
    {
        private readonly ISwitcherService _service;
        private ILogger<SwitcherController> _logger;

        public SwitcherController(ISwitcherService service, ILogger<SwitcherController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Connect([FromQuery] string address)
        {
            _logger.LogInformation($"Attempting connection to {(string.IsNullOrEmpty(address) ? "USB-C" : address)}");
            try
            {
                await _service.Connect(address);
                return Ok();
            }
            catch (ConnectFailureNoResponseException cfnrEx)
            {
                _logger.LogWarning(cfnrEx, cfnrEx.Message);
                return NotFound();
            }
            catch (ConnectFailureIncompatibleFirmwareException cfifEx)
            {
                _logger.LogWarning(cfifEx, cfifEx.Message);
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Disconnect()
        {
            // TODO: Implement this
            await Task.CompletedTask;
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AllowStreamingToResume()
        {
            await _service.AllowStreamingToResume();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> DoesSupportAutoVideoMode()
        {
            var v = await _service.DoesSupportAutoVideoMode();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> DoesSupportDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            var v= await _service.DoesSupportDownConvertedHDVideoMode(from, to);
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> DoesSupportMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            var v= await _service.DoesSupportMultiViewVideoMode(from, to);
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> DoesSupportVideoMode(_BMDSwitcherVideoMode mode)
        {
            var v=await _service.DoesSupportVideoMode(mode);
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> DoesVideoModeChangeRequireReconfiguration(_BMDSwitcherVideoMode mode)
        {
            var v= await _service.DoesVideoModeChangeRequireReconfiguration(mode);
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> Get3GSDIOutputLevel()
        {
            var v=await _service.Get3GSDIOutputLevel();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetAreOutputsConfigurable()
        {
            var v=await _service.GetAreOutputsConfigurable();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetAutoVideoMode()
        {
            var v=await _service.GetAutoVideoMode();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetAutoVideoModeDetected()
        {
            var v= await _service.GetAutoVideoModeDetected();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from)
        {
            var v=await _service.GetDownConvertedHDVideoMode(from);
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetMethodForDownConvertedSD()
        {
            var v=await _service.GetMethodForDownConvertedSD();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetMultiViewVideoMode(_BMDSwitcherVideoMode from)
        {
            var v=await _service.GetMultiViewVideoMode(from);
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetPowerStatus()
        {
            var v= await _service.GetPowerStatus();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductName()
        {
            var v= await _service.GetProductName();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetSuperSourceCascade()
        {
            var v= await _service.GetSuperSourceCascade();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeCode()
        {
            var v= await _service.GetTimeCode();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeCodeLocked()
        {
            var v=await _service.GetTimeCodeLocked();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeCodeMode()
        {
            var v=await _service.GetTimeCodeMode();
            return Ok(v);
        }

        [HttpGet]
        public async Task<IActionResult> GetVideoMode()
        {
            var v= await _service.GetVideoMode();
            return Ok(v);
        }

        [HttpPost]
        public async Task<IActionResult> RequestTimeCode()
        {
            await _service.RequestTimeCode();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Set3GSDIOutputLevel(_BMDSwitcher3GSDIOutputLevel level)
        {
            await _service.Set3GSDIOutputLevel(level);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetAutoVideoMode(bool enabled)
        {
            await _service.SetAutoVideoMode(enabled);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            await _service.SetDownConvertedHDVideoMode(from, to);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetMethodForDownConvertedSD(_BMDSwitcherDownConversionMethod method)
        {
            await _service.SetMethodForDownConvertedSD(method);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            await _service.SetMultiViewVideoMode(from, to);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetSuperSourceCascade(int cascade)
        {
            await _service.SetSuperSourceCascade(cascade);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetTimeCode(TimeCode timeCode)
        {
            await _service.SetTimeCode(timeCode);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetTimeCodeMode(_BMDSwitcherTimeCodeMode mode)
        {
            await _service.SetTimeCodeMode(mode);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetVideoMode(_BMDSwitcherVideoMode mode)
        {
            await _service.SetVideoMode(mode);
            return Ok();
        }
    }
}