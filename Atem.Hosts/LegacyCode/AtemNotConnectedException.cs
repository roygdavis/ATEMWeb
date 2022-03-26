using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Legacy.Exceptions
{

    [Serializable]
    public class AtemNotConnectedException : Exception
    {
        public AtemNotConnectedException() { }
        public AtemNotConnectedException(string message) : base(message) { }
        public AtemNotConnectedException(string message, Exception inner) : base(message, inner) { }
        protected AtemNotConnectedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
