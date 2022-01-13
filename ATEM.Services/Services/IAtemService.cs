using ATEM.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATEM.Services.Services
{
    public interface IAtemService
    {
        Task Connect(string address);
        Task SetPgmInput(int meBlockIndex, long inputId);
        Task SetPvwInput(int meBlockIndex, long inputId);
        Task<IMixEffectBlock> GetMeBlock(int meId);
        Task<IEnumerable<IMixEffectBlock>> GetMeBlocks();
    }
}