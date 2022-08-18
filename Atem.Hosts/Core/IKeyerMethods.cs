using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Core
{
    public interface IKeyerMethods
    {
        Task CanBeDVEKey(out int canDVE);
        Task DoesSupportAdvancedChroma(out int supportsAdvancedChroma);
        Task GetCutInputAvailabilityMask(out _BMDSwitcherInputAvailability availabilityMask);
        Task GetFillInputAvailabilityMask(out _BMDSwitcherInputAvailability availabilityMask);
        Task GetInputCut(out long input);
        Task GetInputFill(out long input);
        Task GetMaskBottom(out double bottom);
        Task GetMasked(out int maskEnabled);
        Task GetMaskLeft(out double left);
        Task GetMaskRight(out double right);
        Task GetMaskTop(out double top);
        Task GetOnAir(out int onAir);
        Task GetTransitionSelectionMask(out _BMDSwitcherTransitionSelection selectionMask);
        Task ResetMask();
        Task SetInputCut(long input);
        Task SetInputFill(long input);
        Task SetMaskBottom(double bottom);
        Task SetMasked(int maskEnabled);
        Task SetMaskLeft(double left);
        Task SetMaskRight(double right);
        Task SetMaskTop(double top);
        Task SetOnAir(int onAir);
        Task SetType(_BMDSwitcherKeyType type);
    }
}
