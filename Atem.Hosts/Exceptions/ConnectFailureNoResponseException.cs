using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Exceptions
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


    
}
