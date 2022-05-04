using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atem.Hosts.Core
{
    public struct TimeCode
    {
        public byte Hours;
        public byte Minutes;
        public byte Seconds;
        public byte Frames;
        public int DroppedFrames;

        public TimeCode(byte hours, byte minutes, byte seconds, byte frames, int droppedFrames)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Frames = frames;
            DroppedFrames = droppedFrames;
        }
    }
}