

using ATEM.Services;
using ATEM.Services.Services;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace ATEMWebApp.Hubs
{
    public class ATEMEventsHub : Hub
    {
        private readonly IAtemService _atem;

        //public ATEMEventsHub() : this(_atem.Instance)
        //{ }

        public ATEMEventsHub(IAtemService atem)
        {
            _atem = atem;
        }

        public async Task<string> GetPGM()
        {
            var meBlock = await _atem.GetMeBlock(0);
            return meBlock?.ProgramInput.ToString() ?? string.Empty;
        }
    }
}