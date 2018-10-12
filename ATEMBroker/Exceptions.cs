using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenMedia.ATEM.Broker
{

    [Serializable]
    public class ConnectFailureNoResponseException : Exception
    {
        public ConnectFailureNoResponseException() { }
        public ConnectFailureNoResponseException(string message) : base(message) { }
        public ConnectFailureNoResponseException(string message, Exception inner) : base(message, inner) { }
        protected ConnectFailureNoResponseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


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
