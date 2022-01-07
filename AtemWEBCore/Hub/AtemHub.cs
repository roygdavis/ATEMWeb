using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATEM.Services;
using Microsoft.AspNetCore.SignalR;

namespace AtemWEBCore.Hubs
{

    public class AtemHub : Hub
    {
        private readonly AtemService _atem;

        public AtemHub(AtemService atem) 
        {
            _atem = atem;
        }
    }
}