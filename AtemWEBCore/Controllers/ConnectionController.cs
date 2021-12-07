using ATEM.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtemWEBCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly Atem _atem;

        public ConnectionController(Atem atem)
        {
            _atem = atem;
        }

        [HttpGet("{address}")]
        public async Task<IActionResult> Connect(string address)
        {
            try
            {
                await Task.Run(() => _atem.Connect(address));
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("/disconnect")]
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
