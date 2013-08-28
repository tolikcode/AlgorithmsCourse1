using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlgorithmsCourse1.TasksImplementations
{
    class TwoSum
    {
        private int twoSumCount;
        private int integersProcessed;

        /// <summary>
        /// Calculates the number of integers in [sumRangeFrom, sumRangeTo] range that represent a sum
        /// of two distinct integers in long[] integers.
        /// </summary>
        /// <remarks>
        /// This approach is too slow. It takes about 15 min on my machine.
        /// There is a faster way to do this with a sorted array (for the case when target sum can be one of the range, not a single number).
        /// 1) Sort all long[] integers array. 
        /// 2) Foreach integer int1 in this array find another int2, so that (int2 bigger than -10000 - int1) && (int2 less than 10000 - int1)
        /// I think this might be faster.
        /// </remarks>
        /// <returns></returns>
        public int Calculate(long[] integers, int sumRangeFrom, int sumRangeTo)
        {
            twoSumCount = 0;
            integersProcessed = 0;

            HashSet<long> integersHashSet = new HashSet<long>(integers);

            Parallel.For(-10000, 10001, i => CheckIsTwoSum(i, integers, integersHashSet));

            return twoSumCount;
        }

        private void CheckIsTwoSum(long targetSum, long[] integers, HashSet<long> integersHashSet)
        {
            foreach (long integer in integers)
            {
                if (integersHashSet.Contains(targetSum - integer) && (targetSum - integer) != integer)
                {
                    Interlocked.Increment(ref twoSumCount);
                    break;
                }
            }
            Interlocked.Increment(ref integersProcessed);
            if (integersProcessed % 100 == 0)
                Console.WriteLine("Number processed " + integersProcessed);
        }
    }
}
