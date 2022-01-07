using System.Collections.Generic;
using ATEM.Services.Interfaces;
using ATEM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ATEMWebApp.Hubs;
using System.Threading.Tasks;

namespace ATEMWebApp.Controllers
{
    [Route("api/atem")]
    [ApiController]
    public class MixEffectsController : ControllerBase
    {
        private readonly AtemInstance _atem;
        private readonly IHubContext<ATEMEventsHub> _hub;

        public MixEffectsController(AtemInstance atem, IHubContext<ATEMEventsHub> hub)
        {
            _atem = atem;
            _hub = hub;
        }


        [HttpGet("mixeffects")]
        public IEnumerable<IMixEffectBlock> Get()
        {
            return _atem.MixEffectsBlocks;
        }

        [HttpGet("mixeffects/{meId}/pgm")]
        [HttpGet("mixeffects/{meId}/pgm/{pgmId}")]
        public async Task<IMixEffectBlock> PGM(int meId, int pgmId = -1)
        {
            // TODO: check meId is not out of bounds
            // TODO: check pgmId is valid

            IMixEffectBlock instance = _atem.MixEffectsBlocks[meId];
            if (pgmId != -1) { instance.ProgramInput = pgmId; }
            await _hub.Clients.All.SendAsync("Notify", instance);
            return instance;
        }

        [HttpGet("mixeffects/{meId}/pvw")]
        [HttpGet("mixeffects/{meId}/pvw/{pvwId}")]
        public IMixEffectBlock PVW(int meId, int pvwId = -1)
        {
            // TODO: check meId is not out of bounds
            // TODO: check pgmId is valid

            IMixEffectBlock instance = _atem.MixEffectsBlocks[meId];
            if (pvwId != -1) { instance.PreviewInput = pvwId; }
            return instance;
        }
    }
}