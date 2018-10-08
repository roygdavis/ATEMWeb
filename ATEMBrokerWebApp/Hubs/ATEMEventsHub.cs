using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATEMWeb.Classes;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ATEMWeb.Hubs
{
    [HubName("atemEvents")]
    public class ATEMEventsHub : Hub
    {
        private readonly AtemHelper _atem;

        public ATEMEventsHub() :
            this(AtemHelper.Instance)
        { }

        public ATEMEventsHub(AtemHelper atem)
        {
            _atem = atem;
        }

        public string GetPGM()
        {
            return _atem.PGM;
        }
    }
}