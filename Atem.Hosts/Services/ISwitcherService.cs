using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Services
{
    public interface ISwitcherService
    {
        Task Connect(string address);
        Task Disconnect();
        Task SetPGM(int switcherIndex, long input);
        Task SetPVW(int switcherIndex, long input);
    }
}