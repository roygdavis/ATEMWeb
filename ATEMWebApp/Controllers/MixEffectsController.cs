using System.Collections.Generic;
//using ATEM.Services.Interfaces;
//using ATEM.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
//using ATEM.Services.Services;
using Atem.Hosts.Switcher;
using Atem.Hosts.Services;
using Atem.Hosts.Core;
using BMDSwitcherAPI;

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

        [HttpGet("{mixEffectBlockId}/GetFadeToBlackFramesRemaining")]        
        public async Task<uint> GetFadeToBlackFramesRemaining(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetFadeToBlackFramesRemaining();
        }

        [HttpGet("{mixEffectBlockId}/GetFadeToBlackFullyBlack")]
        public async Task<int> GetFadeToBlackFullyBlack(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetFadeToBlackFullyBlack();
        }

        [HttpGet("{mixEffectBlockId}/GetFadeToBlackInTransition")]
        public async Task<int> GetFadeToBlackInTransition(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetFadeToBlackInTransition();
        }

        [HttpGet("{mixEffectBlockId}/GetFadeToBlackRate")]
        public async Task<uint> GetFadeToBlackRate(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetFadeToBlackRate();
        }

        [HttpGet("{mixEffectBlockId}/GetInFadeToBlack")]
        public async Task<int> GetInFadeToBlack(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetInFadeToBlack();
        }

        [HttpGet("{mixEffectBlockId}/GetInputAvailabilityMask")]
        public async Task<_BMDSwitcherInputAvailability> GetInputAvailabilityMask(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetInputAvailabilityMask();
        }

        [HttpGet("{mixEffectBlockId}/GetInTransition")]
        public async Task<int> GetInTransition(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetInTransition();
        }

        [HttpGet("{mixEffectBlockId}/GetPreviewInput")]
        public async Task<long> GetPreviewInput(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetPreviewInput();
        }

        [HttpGet("{mixEffectBlockId}/GetPreviewLive")]
        public async Task<int> GetPreviewLive(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetPreviewLive();
        }

        [HttpGet("{mixEffectBlockId}/GetPreviewTransition")]
        public async Task<int> GetPreviewTransition(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetPreviewTransition();
        }

        [HttpGet("{mixEffectBlockId}/GetProgramInput")]
        public async Task<long> GetProgramInput(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetProgramInput();
        }

        [HttpGet("{mixEffectBlockId}/GetTransitionFramesRemaining")]
        public async Task<uint> GetTransitionFramesRemaining(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetTransitionFramesRemaining();
        }

        [HttpGet("{mixEffectBlockId}/GetTransitionPosition")]
        public async Task<double> GetTransitionPosition(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            return await _service.GetTransitionPosition();
        }

        [HttpPost("{mixEffectBlockId}/PerformAutoTransition")]
        public async Task PerformAutoTransition(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            await _service.PerformAutoTransition();
        }

        [HttpPost("{mixEffectBlockId}/PerformCut")]
        public async Task PerformCut(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            await _service.PerformCut();
        }

        [HttpPost("{mixEffectBlockId}/PerformFadeToBlack")]
        public async Task PerformFadeToBlack(int mixEffectBlockId)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
            await _service.PerformFadeToBlack();
        }

        [HttpPost("{mixEffectBlockId}/SetFadeToBlackFullyBlack/{value}")]
        public async Task SetFadeToBlackFullyBlack(int mixEffectBlockId,int value)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
             await _service.SetFadeToBlackFullyBlack(value);
        }

        [HttpPost("{mixEffectBlockId}/SetFadeToBlackRate/{value}")]
        
        public async Task SetFadeToBlackRate(int mixEffectBlockId,uint value)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
             await _service.SetFadeToBlackRate(value);
        }

        [HttpPost("{mixEffectBlockId}/SetPreviewInput/{value}")]
        public async Task SetPreviewInput(int mixEffectBlockId,long value)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
             await _service.SetPreviewInput(value);
        }

        [HttpPost("{mixEffectBlockId}/SetPreviewTransition/{value}")]
        public async Task SetPreviewTransition(int mixEffectBlockId,int value)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
             await _service.SetPreviewTransition(value);
        }

        [HttpPost("{mixEffectBlockId}/SetProgramInput/{value}")]
        public async Task SetProgramInput(int mixEffectBlockId,long value)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
             await _service.SetProgramInput(value);
        }

        [HttpPost("{mixEffectBlockId}/SetTransitionPosition/{value}")]
        public async Task SetTransitionPosition(int mixEffectBlockId,double value)
        {
            await _service.SetMixEffectsContext(mixEffectBlockId);
             await _service.SetTransitionPosition(value);
        }
    }
}