﻿using ATEM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATEMWebApp.Controllers
{
    [Route("api/atem")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly AtemInstance _atem;
        private ILogger<ConnectionController> _logger;

        public ConnectionController(AtemInstance atem, ILogger<ConnectionController> logger)
        {
            _atem = atem;
            _logger = logger;
        }

        [HttpGet("connect/{address?}")]
        public async Task<IActionResult> Connect(string address)
        {
            _logger.LogInformation($"Attempting connection to {(string.IsNullOrEmpty(address) ? "USB-C" : address)}");
            try
            {
                await Task.Run(() => _atem.Connect(address));
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
                await Task.Run(() => _atem.Connect("FOO"));
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
