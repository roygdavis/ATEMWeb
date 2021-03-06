﻿using System;
using System.Configuration;
using ATEMWeb.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SixteenMedia.ATEM.Wrapper;

namespace ATEMWeb.Classes
{
    public class AtemHelper
    {
        // Singleton instance
        private static readonly Lazy<AtemHelper> _instance = new Lazy<AtemHelper>(() => new AtemHelper(GlobalHost.ConnectionManager.GetHubContext<ATEMEventsHub>().Clients));

        public static AtemHelper Instance => _instance.Value;

        public Atem Atem { get; private set; }
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
            Atem = new Atem();
            Atem.Connected += Atem_Connected;
            Atem.MixEffectBlockConnectedEvent += Atem_MixEffectBlockConnectedEvent;
            Atem.DisconnectedEvent += Atem_Disconnected;
            Atem.Connect(ConfigurationManager.AppSettings["atemIP"]);
        }

        private void Atem_MixEffectBlockConnectedEvent(object sender, MixEffectBlockConnectedEventArgs e)
        {
            e.ConnectedMEBlock.ProgramInputChanged += ConnectedMEBlock_ProgramInputChanged;
            e.ConnectedMEBlock.PreviewInputChanged += ConnectedMEBlock_PreviewInputChanged;
            e.ConnectedMEBlock.InTransitionChanged += ConnectedMEBlock_InTransitionChanged;
            e.ConnectedMEBlock.TransitionFramesRemainingChanged += ConnectedMEBlock_TransitionFramesRemainingChanged;
            e.ConnectedMEBlock.TransitionPositionChanged += ConnectedMEBlock_TransitionPositionChanged;
            e.ConnectedMEBlock.FadeToBlackFramesRemainingChanged += ConnectedMEBlock_FadeToBlackFramesRemainingChanged;
            e.ConnectedMEBlock.FadeToBlackFullyBlackChanged += ConnectedMEBlock_FadeToBlackFullyBlackChanged;
            e.ConnectedMEBlock.FadeToBlackInTransitionChanged += ConnectedMEBlock_FadeToBlackInTransitionChanged;
            e.ConnectedMEBlock.FadeToBlackRateChanged += ConnectedMEBlock_FadeToBlackRateChanged;
            e.ConnectedMEBlock.InFadeToBlackChanged += ConnectedMEBlock_InFadeToBlackChanged;
            e.ConnectedMEBlock.InputAvailabilityMaskChanged += ConnectedMEBlock_InputAvailabilityMaskChanged;
            e.ConnectedMEBlock.PreviewLiveChanged += ConnectedMEBlock_PreviewLiveChanged;
            e.ConnectedMEBlock.PreviewTransitionChanged += ConnectedMEBlock_PreviewTransitionChanged;
        }

        private void ConnectedMEBlock_PreviewTransitionChanged(object sender, MixEffectsEventArgs e)
        {
            Clients.All.notifyAtemMixEffectsEvent(new 
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_PreviewLiveChanged(object sender, MixEffectsEventArgs e)
        {
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_InputAvailabilityMaskChanged(object sender, MixEffectsEventArgs e)
        {
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_InFadeToBlackChanged(object sender, MixEffectsEventArgs e)
        {
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_FadeToBlackRateChanged(object sender, MixEffectsEventArgs e)
        {
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_FadeToBlackInTransitionChanged(object sender, MixEffectsEventArgs e)
        {
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_FadeToBlackFullyBlackChanged(object sender, MixEffectsEventArgs e)
        {
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_FadeToBlackFramesRemainingChanged(object sender, MixEffectsEventArgs e)
        {
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_TransitionPositionChanged(object sender, MixEffectsEventArgs e)
        {
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_TransitionFramesRemainingChanged(object sender, MixEffectsEventArgs e)
        {
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_InTransitionChanged(object sender, MixEffectsEventArgs e)
        {
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_PreviewInputChanged(object sender, MixEffectsEventArgs e)
        {
            // Clients.All.notifyAtemPVWEvent(string.Format("PVW Changed: {0}", e.Input));
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void ConnectedMEBlock_ProgramInputChanged(object sender, MixEffectsEventArgs e)
        {
            // Clients.All.notifyAtemMixEffectsEvent(string.Format("PGM Changed: {0}", e.Input));
            Clients.All.notifyAtemMixEffectsEvent(new
            {
                MixEffects = e
            });
        }

        private void Atem_Disconnected(object sender, EventArgs e)
        {
            Clients.All.notifyAtemEvent("ATEM Disconnected");
        }

        private void Atem_Connected(object sender, EventArgs e)
        {
            Clients.All.notifyAtemEvent("ATEM Connected");
        }
    }
}