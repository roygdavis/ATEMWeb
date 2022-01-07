using System.Collections.Generic;
using ATEM.Services.Interfaces;
using ATEM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ATEMWebApp.Hubs;
using System.Threading.Tasks;

namespace ATEMWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MixEffectsController : ControllerBase
    {
        private readonly AtemService _atem;
        private readonly IHubContext<ATEMEventsHub> _hub;

        public MixEffectsController(AtemService atem, IHubContext<ATEMEventsHub> hub)
        {
            _atem = atem;
            _hub = hub;
        }


        [Route("api/atem/mixeffects")]
        public IEnumerable<IMixEffectBlock> Get()
        {
            return _atem.MixEffectsBlocks;
        }

        [Route("api/atem/mixeffects/{meId}/pgm")]
        [Route("api/atem/mixeffects/{meId}/pgm/{pgmId}")]
        [HttpGet]
        public async Task<IMixEffectBlock> PGM(int meId, int pgmId = -1)
        {
            // TODO: check meId is not out of bounds
            // TODO: check pgmId is valid

            IMixEffectBlock instance = _atem.MixEffectsBlocks[meId];
            if (pgmId != -1) { instance.ProgramInput = pgmId; }
            await _hub.Clients.All.SendAsync("Notify", instance);
            return instance;
        }

        [Route("api/atem/mixeffects/{meId}/pvw")]
        [Route("api/atem/mixeffects/{meId}/pvw/{pvwId}")]
        [HttpGet]
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