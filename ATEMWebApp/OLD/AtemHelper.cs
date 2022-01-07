//using System;
//using ATEM.Services;
//using ATEM.Services.Services;
//using ATEMWebApp.Hubs;
//using Microsoft.AspNetCore.SignalR;

//namespace ATEMWebApp.Classes
//{
//    public class AtemHelper
//    {
//        private readonly AtemService _atem;
//        private readonly IHubContext<ATEMEventsHub> _hubContext;

//        public string PGM { get; internal set; }

//        private void Atem_MixEffectBlockConnectedEvent(object sender, MixEffectBlockConnectedEventArgs e)
//        {
//            e.ConnectedMEBlock.ProgramInputChanged += ConnectedMEBlock_ProgramInputChanged;
//            e.ConnectedMEBlock.PreviewInputChanged += ConnectedMEBlock_PreviewInputChanged;
//            e.ConnectedMEBlock.InTransitionChanged += ConnectedMEBlock_InTransitionChanged;
//            e.ConnectedMEBlock.TransitionFramesRemainingChanged += ConnectedMEBlock_TransitionFramesRemainingChanged;
//            e.ConnectedMEBlock.TransitionPositionChanged += ConnectedMEBlock_TransitionPositionChanged;
//            e.ConnectedMEBlock.FadeToBlackFramesRemainingChanged += ConnectedMEBlock_FadeToBlackFramesRemainingChanged;
//            e.ConnectedMEBlock.FadeToBlackFullyBlackChanged += ConnectedMEBlock_FadeToBlackFullyBlackChanged;
//            e.ConnectedMEBlock.FadeToBlackInTransitionChanged += ConnectedMEBlock_FadeToBlackInTransitionChanged;
//            e.ConnectedMEBlock.FadeToBlackRateChanged += ConnectedMEBlock_FadeToBlackRateChanged;
//            e.ConnectedMEBlock.InFadeToBlackChanged += ConnectedMEBlock_InFadeToBlackChanged;
//            e.ConnectedMEBlock.InputAvailabilityMaskChanged += ConnectedMEBlock_InputAvailabilityMaskChanged;
//            e.ConnectedMEBlock.PreviewLiveChanged += ConnectedMEBlock_PreviewLiveChanged;
//            e.ConnectedMEBlock.PreviewTransitionChanged += ConnectedMEBlock_PreviewTransitionChanged;
//        }

//        private void ConnectedMEBlock_PreviewTransitionChanged(object sender, MixEffectsEventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new 
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_PreviewLiveChanged(object sender, MixEffectsEventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_InputAvailabilityMaskChanged(object sender, MixEffectsEventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_InFadeToBlackChanged(object sender, MixEffectsEventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_FadeToBlackRateChanged(object sender, MixEffectsEventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_FadeToBlackInTransitionChanged(object sender, MixEffectsEventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_FadeToBlackFullyBlackChanged(object sender, MixEffectsEventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_FadeToBlackFramesRemainingChanged(object sender, MixEffectsEventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_TransitionPositionChanged(object sender, MixEffectsEventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_TransitionFramesRemainingChanged(object sender, MixEffectsEventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_InTransitionChanged(object sender, MixEffectsEventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_PreviewInputChanged(object sender, MixEffectsEventArgs e)
//        {
//            // _hubContext.Clients.All.notifyAtemPVWEvent(string.Format("PVW Changed: {0}", e.Input));
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void ConnectedMEBlock_ProgramInputChanged(object sender, MixEffectsEventArgs e)
//        {
//            // _hubContext.Clients.All.notifyAtemMixEffectsEvent(string.Format("PGM Changed: {0}", e.Input));
//            _hubContext.Clients.All.notifyAtemMixEffectsEvent(new
//            {
//                MixEffects = e
//            });
//        }

//        private void Atem_Disconnected(object sender, EventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemEvent("ATEM Disconnected");
//        }

//        private void Atem_Connected(object sender, EventArgs e)
//        {
//            _hubContext.Clients.All.notifyAtemEvent("ATEM Connected");
//        }
//    }
//}