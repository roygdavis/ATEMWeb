using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ATEMWeb.Classes;
using SixteenMedia.ATEM.Wrapper;

namespace ATEMWeb.Controllers
{

    public class MixEffectsController : ApiController
    {
        [Route("api/atem/mixeffects")]
        public IEnumerable<SixteenMedia.ATEM.Wrapper.MEBlock> Get()
        {
            return AtemHelper.Instance.Atem.MixEffectsBlocks;
        }

        [Route("api/atem/mixeffects/{meId}/pgm")]
        [Route("api/atem/mixeffects/{meId}/pgm/{pgmId}")]
        [HttpGet]
        public SixteenMedia.ATEM.Wrapper.MEBlock PGM(int meId, int pgmId = -1)
        {
            // TODO: check meId is not out of bounds
            // TODO: check pgmId is valid

            MEBlock instance = AtemHelper.Instance.Atem.MixEffectsBlocks[meId];
            if (pgmId != -1) { instance.ProgramInput = pgmId; }
            return instance;
        }

        [Route("api/atem/mixeffects/{meId}/pvw")]
        [Route("api/atem/mixeffects/{meId}/pvw/{pvwId}")]
        [HttpGet]
        public SixteenMedia.ATEM.Wrapper.MEBlock PVW(int meId, int pvwId = -1)
        {
            // TODO: check meId is not out of bounds
            // TODO: check pgmId is valid

            MEBlock instance = AtemHelper.Instance.Atem.MixEffectsBlocks[meId];
            if (pvwId != -1) { instance.PreviewInput = pvwId; }
            return instance;
        }
    }
}