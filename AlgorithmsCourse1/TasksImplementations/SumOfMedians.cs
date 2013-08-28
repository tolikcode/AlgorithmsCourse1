using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmsCourse1.DataStructures;

namespace AlgorithmsCourse1.TasksImplementations
{
    /// <summary>
    /// Determines a mediane after every next integer in an array, and then sums all medianes.
    /// </summary>
    class SumOfMedians
    {
        public int Calculate(int[] integers)
        {
            Heap<int> lowerHeap = new Heap<int>(true);
            Heap<int> upperHeap = new Heap<int>(false);

            int sumOfMedians = 0;

            lowerHeap.Insert(integers[0]);
            sumOfMedians += integers[0];
            for (int i = 1; i < integers.Length; i++)
            {
                if (integers[i] < lowerHeap.PeekRoot())
                {
                    lowerHeap.Insert(integers[i]);
                }
                else
                {
                    upperHeap.Insert(integers[i]);
                }

                if (upperHeap.Count - lowerHeap.Count > 1)
                {
                    lowerHeap.Insert(upperHeap.ExtractRoot());
                }
                else if (lowerHeap.Count - upperHeap.Count > 1)
                {
                    upperHeap.Insert(lowerHeap.ExtractRoot());
                }

                if (upperHeap.Count > lowerHeap.Count)
                {
                    sumOfMedians += upperHeap.PeekRoot();
                }
                else
                {
                    sumOfMedians += lowerHeap.PeekRoot();
                }
            }

            return sumOfMedians;
        }

    }
}
