﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace godor
{
    class Pit
    {
        public SortedDictionary<int, Measurement> Measurements { get; set; } = new();

        public Pit()
        {

        }
    }
}
