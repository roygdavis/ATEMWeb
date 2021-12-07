using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATEM.Wrapper;
using Microsoft.AspNetCore.SignalR;

namespace AtemWEBCore.Hubs
{

    public class AtemHub : Hub
    {
        private readonly Atem _atem;

        public AtemHub(Atem atem) 
        {
            _atem = atem;
        }
    }
}