using BMDSwitcherAPI;
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
        Task<long> GetProgramInput();

        ///
        ///  Set the program input.
        ///
        Task SetProgramInput(long value);

        ///
        ///  Get the current preview input.
        ///
        Task<long> GetPreviewInput();

        ///
        ///  Set the preview input.
        ///
        Task SetPreviewInput(long value);

        ///
        ///  Get the current preview-live flag.
        ///
        Task<int> GetPreviewLive();

        ///
        ///  Get the current preview-transition flag.
        ///
        Task<int> GetPreviewTransition();

        ///
        ///  Set the preview-transition flag.
        ///
        Task SetPreviewTransition(int value);

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
        Task<int> GetInTransition();

        ///
        ///  Get the current transition position value.
        ///
        Task<double> GetTransitionPosition();

        ///
        ///  Set the transition position value.
        ///
        Task SetTransitionPosition(double value);

        ///
        ///  Get the number of transition frames remaining.
        ///
        Task<uint> GetTransitionFramesRemaining();

        ///
        ///  Initiate a fade to black.
        ///
        Task PerformFadeToBlack();

        ///
        ///  Get the current fade to black rate value.
        ///
        Task<uint> GetFadeToBlackRate();

        ///
        ///  Set the fade to black rate value.
        ///
        Task SetFadeToBlackRate(uint value);

        ///
        ///  Get the current number of fade to black frames remaining.
        ///
        Task<uint> GetFadeToBlackFramesRemaining();

        ///
        ///  Get the current fade-to-black-fully-black flag.
        ///
        Task<int> GetFadeToBlackFullyBlack();

        ///
        ///  Set the fade-to-black-fully-black flag.
        ///
        Task SetFadeToBlackFullyBlack(int value);

        ///
        ///  Get the current in-fade-to-black flag.
        ///
        Task<int> GetInFadeToBlack();

        ///
        ///  Get the current fade-to-black-in-transition flag.
        ///
        Task<int> GetFadeToBlackInTransition();

        ///
        ///  Get the switcher input availability mask.
        ///
        Task<_BMDSwitcherInputAvailability> GetInputAvailabilityMask();
    }
}