using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Exceptions
{

    [Serializable]
    public class MixEffectsNullException : Exception
    {
        public MixEffectsNullException() { }
        public MixEffectsNullException(string message) : base(message) { }
        public MixEffectsNullException(string message, Exception inner) : base(message, inner) { }
        protected MixEffectsNullException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
