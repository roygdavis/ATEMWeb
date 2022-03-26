using Atem.Hosts.Exceptions;
using Atem.Hosts.Services;
using Atem.Hosts.Switcher;
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
    [Route("atem")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly ISwitcherService _service;
        private ILogger<ConnectionController> _logger;

        public ConnectionController(ISwitcherService service, ILogger<ConnectionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("connect")]
        public async Task<IActionResult> Connect([FromQuery]string address)
        {
            _logger.LogInformation($"Attempting connection to {(string.IsNullOrEmpty(address) ? "USB-C" : address)}");
            try
            {
                await _service.Connect(address);
                return Ok();
            }
            catch(ConnectFailureNoResponseException cfnrEx)
            {
                _logger.LogWarning(cfnrEx, cfnrEx.Message);
                return NotFound();
            } 
            catch(ConnectFailureIncompatibleFirmwareException cfifEx)
            {
                _logger.LogWarning(cfifEx, cfifEx.Message);
                return StatusCode(500);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("disconnect")]
        public async Task<IActionResult> Disconnect()
        {
            try
            {
                await Task.Run(() => _service.Connect("FOO"));
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
