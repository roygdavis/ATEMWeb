

using Atem.Hosts.Legacy;
using Atem.Hosts.Legacy.Services;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace Atem.Hosts.Legacy.Hubs
{
    public class ATEMEventsHub : Hub
    {

        //public ATEMEventsHub() : this(_atem.Instance)
        //{ }

        //public ATEMEventsHub(IAtemService atem)
        //{
        //    _atem = atem;
        //}

        //public async Task<string> GetPGM()
        //{
        //    var meBlock = await _atem.GetMeBlock(0);
        //    return meBlock?.ProgramInput.ToString() ?? string.Empty;
        //}

    }
}