using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse1
{
    class CountedInversionsIntArray
    {
        public int[] Array { get; set; }
        public long NumberOfInversions { get; set; }

        public int this[int i]
        {
            get { return Array[i]; }
            set { Array[i] = value; }
        }

        public int Length
        {
            get { return Array.Length; }
        }

        
    }
}
