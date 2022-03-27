using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Core
{
    public interface IMixEffectsMethods
    {
        ///
        ///  Get the current program input.
        ///
        Task GetProgramInput();

        ///
        ///  Set the program input.
        ///
        Task SetProgramInput();

        ///
        ///  Get the current preview input.
        ///
        Task GetPreviewInput();

        ///
        ///  Set the preview input.
        ///
        Task SetPreviewInput();

        ///
        ///  Get the current preview-live flag.
        ///
        Task GetPreviewLive();

        ///
        ///  Get the current preview-transition flag.
        ///
        Task GetPreviewTransition();

        ///
        ///  Set the preview-transition flag.
        ///
        Task SetPreviewTransition();

        ///
        ///  Initiate an automatic transition.
        ///
        Task PerformAutoTransition();

        ///
        ///  Initiate a cut.
        ///
        Task PerformCut();

        ///
        ///  Get the current in-transition flag.
        ///
        Task GetInTransition();

        ///
        ///  Get the current transition position value.
        ///
        Task GetTransitionPosition();

        ///
        ///  Set the transition position value.
        ///
        Task SetTransitionPosition();

        ///
        ///  Get the number of transition frames remaining.
        ///
        Task GetTransitionFramesRemaining();

        ///
        ///  Initiate a fade to black.
        ///
        Task PerformFadeToBlack();

        ///
        ///  Get the current fade to black rate value.
        ///
        Task GetFadeToBlackRate();

        ///
        ///  Set the fade to black rate value.
        ///
        Task SetFadeToBlackRate();

        ///
        ///  Get the current number of fade to black frames remaining.
        ///
        Task GetFadeToBlackFramesRemaining();

        ///
        ///  Get the current fade-to-black-fully-black flag.
        ///
        Task GetFadeToBlackFullyBlack();

        ///
        ///  Set the fade-to-black-fully-black flag.
        ///
        Task SetFadeToBlackFullyBlack();

        ///
        ///  Get the current in-fade-to-black flag.
        ///
        Task GetInFadeToBlack();

        ///
        ///  Get the current fade-to-black-in-transition flag.
        ///
        Task GetFadeToBlackInTransition();

        ///
        ///  Get the switcher input availability mask.
        ///
        Task GetInputAvailabilityMask();

    }
}