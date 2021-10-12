using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace godor
{
    class Measurement
    {
        public int Location { get; set; } = 0;
        public int Depth { get; set; } = 0;
        public Pit Pit { get; set; } = null;

        public Measurement()
        {

        }

        public Measurement(int location, int depth)
        {
            Location = location;
            Depth = depth;
        }
    }
}
