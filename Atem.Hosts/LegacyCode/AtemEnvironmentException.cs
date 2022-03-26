using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Legacy.Exceptions
{

    [Serializable]
    public class AtemEnvironmentException : Exception
    {
        public AtemEnvironmentException() { }
        public AtemEnvironmentException(string message) : base(message) { }
        public AtemEnvironmentException(string message, Exception inner) : base(message, inner) { }
        protected AtemEnvironmentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
