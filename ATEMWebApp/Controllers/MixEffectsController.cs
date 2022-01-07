using System.Collections.Generic;
using ATEM.Services.Interfaces;
using ATEM.Services;
using Microsoft.AspNetCore.Mvc;

namespace ATEMWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MixEffectsController : ControllerBase
    {
        private readonly AtemService _atem;

        public MixEffectsController(AtemService atem)
        {
            _atem = atem;
        }


        [Route("api/atem/mixeffects")]
        public IEnumerable<IMixEffectBlock> Get()
        {
            return _atem.MixEffectsBlocks;
        }

        [Route("api/atem/mixeffects/{meId}/pgm")]
        [Route("api/atem/mixeffects/{meId}/pgm/{pgmId}")]
        [HttpGet]
        public IMixEffectBlock PGM(int meId, int pgmId = -1)
        {
            // TODO: check meId is not out of bounds
            // TODO: check pgmId is valid

            IMixEffectBlock instance = _atem.MixEffectsBlocks[meId];
            if (pgmId != -1) { instance.ProgramInput = pgmId; }
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