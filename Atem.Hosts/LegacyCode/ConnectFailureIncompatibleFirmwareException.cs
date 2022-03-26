using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Legacy
{
    [Serializable]
    public class ConnectFailureIncompatibleFirmwareException : Exception
    {
        public ConnectFailureIncompatibleFirmwareException() { }
        public ConnectFailureIncompatibleFirmwareException(string message) : base(message) { }
        public ConnectFailureIncompatibleFirmwareException(string message, Exception inner) : base(message, inner) { }
        protected ConnectFailureIncompatibleFirmwareException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
