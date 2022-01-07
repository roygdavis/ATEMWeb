

using ATEM.Services;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace ATEMWebApp.Hubs
{
    public class ATEMEventsHub : Hub
    {
        private readonly AtemInstance _atem;

        //public ATEMEventsHub() : this(_atem.Instance)
        //{ }

        public ATEMEventsHub(AtemInstance atem)
        {
            _atem = atem;
        }

        public string GetPGM()
        {
            return _atem.MixEffectsBlocks.FirstOrDefault()?.ProgramInput.ToString() ?? string.Empty;
        }
    }
}