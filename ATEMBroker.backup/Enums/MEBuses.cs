using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SixteenMedia.ATEM.Wrapper
{
    [DataContract]
    public enum MEBuses
    {
        [EnumMember(Value = "None")]
        None,

        [EnumMember(Value = "Program")]
        Program,

        [EnumMember(Value = "Preview")]
        Preview
    }
}
