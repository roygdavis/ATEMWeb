using Atem.Hosts.Legacy.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atem.Hosts.Legacy.Services
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