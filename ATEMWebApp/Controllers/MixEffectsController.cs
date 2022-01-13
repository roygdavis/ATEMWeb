using System.Collections.Generic;
using ATEM.Services.Interfaces;
using ATEM.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ATEM.Services.Services;

namespace ATEMWebApp.Controllers
{
    [Route("atem")]
    [ApiController]
    public class MixEffectsController : ControllerBase
    {
        private readonly IAtemService _atem;

        public MixEffectsController(IAtemService atem)
        {
            _atem = atem;
        }

        [HttpGet("mixeffects")]
        public async Task<IEnumerable<IMixEffectBlock>> Get()
        {
            return await _atem.GetMeBlocks();
        }

        [HttpGet("mixeffects/{meId}/pgm")]
        public async Task<IMixEffectBlock> PGM(int meId = 0)
        {
            return await _atem.GetMeBlock(meId);
        }

        [HttpPost("mixeffects/{meId}/pgm/{pgmId}")]
        public async Task PGM(int meId = 0, int pgmId = 1)
        {
            await _atem.SetPgmInput(meId, pgmId);
        }

        [HttpPost("mixeffects/{meId}/pvw/{pvwId}")]
        public async Task PVW(int meId = 0, int pvwId = 1)
        {
            await _atem.SetPvwInput(meId, pvwId);
        }
    }
}