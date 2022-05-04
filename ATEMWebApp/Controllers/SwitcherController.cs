using Atem.Hosts.Core;
using Atem.Hosts.Exceptions;
using Atem.Hosts.Services;
using Atem.Hosts.Switcher;
using ATEMWebApp.Dtos;
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
    [Route("api/atem/[action]")]
    public class SwitcherController : ControllerBase
    {
        private readonly ISwitcherService _service;
        private ILogger<SwitcherController> _logger;

        public SwitcherController(ISwitcherService service, ILogger<SwitcherController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Connects to a Blackmagic Design Atem Switcher
        /// </summary>
        /// <param name="address">The IP or FQDN of the switcher.  If connecting via USB then omit this value.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Connect([FromQuery] string address)
        {
            _logger.LogInformation($"Attempting connection to {(string.IsNullOrEmpty(address) ? "USB-C" : address)}");
            try
            {
                await _service.Connect(address);
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (ConnectFailureNoResponseException cfnrEx)
            {
                _logger.LogWarning(cfnrEx, cfnrEx.Message);
                return NotFound(ResponseDto<ConnectFailureNoResponseException>.CreateErrorResponse("Unable to connect", cfnrEx));
            }
            catch (ConnectFailureIncompatibleFirmwareException cfifEx)
            {
                _logger.LogWarning(cfifEx, cfifEx.Message);
                return StatusCode(500, ResponseDto<ConnectFailureIncompatibleFirmwareException>.CreateErrorResponse("Unable to connect", cfifEx));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unable to connect", ex));
            }
        }

        /// <summary>
        /// Disconnects from a connected Blackmagic Design Atem Switcher
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Disconnect()
        {
            // TODO: Implement this
            try
            {
                await Task.CompletedTask;
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", new Exception()));
            }
        }

        /// <summary>
        /// This method is not documented within ATEM Switchers SDK v 8.6.4 documentation
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public async Task<IActionResult> AllowStreamingToResume()
        {
            try
            {
                await _service.AllowStreamingToResume();
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The DoesSupportAutoVideoMode method determines if auto video mode is supported by the switcher.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DoesSupportAutoVideoMode()
        {
            try
            {
                var v = await _service.DoesSupportAutoVideoMode();
                return Ok(ResponseDto<bool>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The DoesSupportDownConvertedHDVideoMode method determines if a down converted HD output video standard is supported by a particular core video standard
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DoesSupportDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            try
            {
                var v = await _service.DoesSupportDownConvertedHDVideoMode(from, to);
                return Ok(ResponseDto<bool>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The DoesSupportMultiViewVideoMode method determines if a MultiView video standard is supported for a particular core video standard
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DoesSupportMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            try
            {
                var v = await _service.DoesSupportMultiViewVideoMode(from, to);
                return Ok(ResponseDto<bool>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The DoesSupportVideoMode method determines if a video standard is supported by the switcher.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DoesSupportVideoMode(_BMDSwitcherVideoMode mode)
        {
            try
            {
                var v = await _service.DoesSupportVideoMode(mode);
                return Ok(ResponseDto<bool>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The DoesVideoModeChangeRequireReconfiguration method determines if changing to the specified video standard will cause the switcher to be reconfigured, which may result in the switcher restarting.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DoesVideoModeChangeRequireReconfiguration(_BMDSwitcherVideoMode mode)
        {
            try
            {
                var v = await _service.DoesVideoModeChangeRequireReconfiguration(mode);
                return Ok(ResponseDto<bool>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The Get3GSDIOutputLevel method gets the output encoding level for all 3G-SDI outputs of the switcher, on models supporting 3G-SDI video formats.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get3GSDIOutputLevel()
        {
            try
            {
                var v = await _service.Get3GSDIOutputLevel();
                return Ok(ResponseDto<_BMDSwitcher3GSDIOutputLevel>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// <para>The GetAreOutputsConfigurable method indicates whether all of the switcher’s outputs can be configured.</para>
        /// <para>Some switchers have mostly fixed outputs and only a small number of configurable outputs.</para>
        /// <para>Other switchers only have configurable outputs</para>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAreOutputsConfigurable()
        {
            try
            {
                var v = await _service.GetAreOutputsConfigurable();
                return Ok(ResponseDto<bool>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetAutoVideoMode method indicates whether auto video mode is currently enabled.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAutoVideoMode()
        {
            try
            {
                var v = await _service.GetAutoVideoMode();
                return Ok(ResponseDto<bool>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetAutoVideoModeDetected method indicates whether an input video mode has been detected.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAutoVideoModeDetected()
        {
            try
            {
                var v = await _service.GetAutoVideoModeDetected();
                return Ok(ResponseDto<bool>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetDownConvertedHDVideoMode method gets the down converted HD output video standard for a particular core video standard.
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from)
        {
            try
            {
                var v = await _service.GetDownConvertedHDVideoMode(from);
                return Ok(ResponseDto<_BMDSwitcherVideoMode>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetMethodForDownConvertedSD method gets the SD conversion method applied when down converting between broadcast standards.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMethodForDownConvertedSD()
        {
            try
            {
                var v = await _service.GetMethodForDownConvertedSD();
                return Ok(ResponseDto<_BMDSwitcherDownConversionMethod>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetMultiViewVideoMode method gets the MultiView video standard for a particular core video standard.
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMultiViewVideoMode(_BMDSwitcherVideoMode from)
        {
            try
            {
                var v = await _service.GetMultiViewVideoMode(from);
                return Ok(ResponseDto<_BMDSwitcherVideoMode>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetPowerStatus method gets the connected power status, useful for models supporting multiple power sources.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPowerStatus()
        {
            try
            {
                var v = await _service.GetPowerStatus();
                return Ok(ResponseDto<_BMDSwitcherPowerStatus>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetProductName method gets the product name of the switcher.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductName()
        {
            try
            {
                var v = await _service.GetProductName();
                return Ok(ResponseDto<string>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetSuperSourceCascade method indicates whether the SuperSource cascade mode is currently enabled.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSuperSourceCascade()
        {
            try
            {
                var v = await _service.GetSuperSourceCascade();
                return Ok(ResponseDto<bool>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetTimeCode method returns the timecode that was last received from the switcher.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTimeCode()
        {
            try
            {
                var v = await _service.GetTimeCode();
                return Ok(ResponseDto<TimeCode>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetTimeCodeLocked method indicates whether the timecode can be changed with SetTimeCode.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTimeCodeLocked()
        {
            try
            {
                var v = await _service.GetTimeCodeLocked();
                return Ok(ResponseDto<bool>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetTimeCodeMode method returns the current timecode mode of the switcher.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTimeCodeMode()
        {
            try
            {
                var v = await _service.GetTimeCodeMode();
                return Ok(ResponseDto<_BMDSwitcherTimeCodeMode>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetVideoMode method gets the current video standard applied across the switcher.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVideoMode()
        {
            try
            {
                var v = await _service.GetVideoMode();
                return Ok(ResponseDto<_BMDSwitcherVideoMode>.CreateOkResponse(v));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// <para>The RequestTimeCode method requests the current timecode from the switcher which will be cached when received.</para>
        /// <para>Use the GetTimeCode method to get the cached timecode.</para>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RequestTimeCode()
        {
            try
            {
                await _service.RequestTimeCode();
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The Set3GSDIOutputLevel method sets the output encoding level for all 3G-SDI outputs of the switcher, on models supporting 3G-SDI video formats.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Set3GSDIOutputLevel(_BMDSwitcher3GSDIOutputLevel level)
        {
            try
            {
                await _service.Set3GSDIOutputLevel(level);
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The GetTimeCode method is used to enable or disable auto video mode.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetAutoVideoMode(bool enabled)
        {
            try
            {
                await _service.SetAutoVideoMode(enabled);
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetDownConvertedHDVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            try
            {
                await _service.SetDownConvertedHDVideoMode(from, to);
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The SetDownConvertedHDVideoMode method sets the down converted HD output video standard for a particular core video standard.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetMethodForDownConvertedSD(_BMDSwitcherDownConversionMethod method)
        {
            try
            {
                await _service.SetMethodForDownConvertedSD(method);
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The SetMultiViewVideoMode method gets the MultiView video standard for a particular core video standard.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetMultiViewVideoMode(_BMDSwitcherVideoMode from, _BMDSwitcherVideoMode to)
        {
            try
            {
                await _service.SetMultiViewVideoMode(from, to);
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The SetSuperSourceCascade method is used to enable or disable the SuperSource cascade mode.
        /// </summary>
        /// <param name="cascade"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetSuperSourceCascade(int cascade)
        {
            try
            {
                await _service.SetSuperSourceCascade(cascade);
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The SetTimeCode method sets the timecode of the switcher.
        /// </summary>
        /// <param name="timeCode"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetTimeCode(TimeCode timeCode)
        {
            await _service.SetTimeCode(timeCode);
            return Ok(ResponseDto<bool>.CreateOkResponse(true));
        }

        /// <summary>
        /// The SetTimeCodeMode method sets the timecode mode of the switcher.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetTimeCodeMode(_BMDSwitcherTimeCodeMode mode)
        {
            try
            {
                await _service.SetTimeCodeMode(mode);
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }

        /// <summary>
        /// The SetVideoMode method sets the video standard applied across the switcher.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetVideoMode(_BMDSwitcherVideoMode mode)
        {
            try
            {
                await _service.SetVideoMode(mode);
                return Ok(ResponseDto<bool>.CreateOkResponse(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseDto<Exception>.CreateErrorResponse("Unspecified error", ex));
            }
        }
    }
}