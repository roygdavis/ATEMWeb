using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using ATEMWeb.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ATEMWeb.Classes
{
    public class AtemHelper
    {
        // Singleton instance
        private static readonly Lazy<AtemHelper> _instance = new Lazy<AtemHelper>(() => new AtemHelper(GlobalHost.ConnectionManager.GetHubContext<ATEMEventsHub>().Clients));

        public static AtemHelper Instance => _instance.Value;

        public SixteenMedia.ATEM.Broker.Atem Atem { get; private set; }
        public string PGM { get; internal set; }

        public IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ATEMEventsHub>();

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        private AtemHelper(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            Atem = new SixteenMedia.ATEM.Broker.Atem();
            Atem.Connected += Atem_Connected;
            Atem.Disconnected += Atem_Disconnected;
            Atem.PGMInputChanged += Atem_PGMInputChanged;
            Atem.PVWInputChanged += Atem_PVWInputChanged;
            Atem.Connect(ConfigurationManager.AppSettings["atemIP"]);
        }

        private void Atem_PVWInputChanged(object sender, EventArgs e)
        {
            Clients.All.notifyAtemPVWEvent("PVW Changed");
        }

        private void Atem_PGMInputChanged(object sender, EventArgs e)
        {
            PGM = "100";
            Clients.All.notifyAtemPGMEvent("PGM Changed");
        }

        private void Atem_Disconnected(object sender, EventArgs e)
        {
            Clients.All.notifyAtemConEvent("ATEM Disconnected");
        }

        private void Atem_Connected(object sender, EventArgs e)
        {
            Clients.All.notifyAtemConEvent("ATEM Connected");
        }
    }
}