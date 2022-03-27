using System.Collections.Generic;
//using ATEM.Services.Interfaces;
//using ATEM.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
//using ATEM.Services.Services;
using Atem.Hosts.Switcher;
using Atem.Hosts.Services;

namespace ATEMWebApp.Controllers
{
    [Route("api/atem/mixeffects")]
    [ApiController]
    public class MixEffectsController : ControllerBase
    {
        private readonly ISwitcherService _service;

        public MixEffectsController(ISwitcherService service)
        {
            _service = service;
        }

        //[HttpGet("mixeffects")]
        //public async Task<IEnumerable<IMixEffectBlock>> Get()
        //{
        //    return await _service.GetMeBlocks();
        //}

        //[HttpGet("mixeffects/{meId}/pgm")]
        //public async Task<IMixEffectBlock> PGM(int meId = 0)
        //{
        //    return await _service.GetMeBlock(meId);
        //}

        [HttpPost("mixeffects/{meId}/pgm/{pgmId}")]
        public async Task PGM(int meId = 0, int pgmId = 1)
        {
            await _service.SetPGM(meId, pgmId);
        }

        [HttpPost("mixeffects/{meId}/pvw/{pvwId}")]
        public async Task PVW(int meId = 0, int pvwId = 1)
        {
            await _service.SetPVW(meId, pvwId);
        }
    }
}