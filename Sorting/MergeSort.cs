using System;
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

    class MergeSort
    {
        public static int[] Sort(int[] initialArray, bool descending)
        {
            if (initialArray.Length < 2)
                return initialArray;

            int[] firstArray = initialArray.Take(initialArray.Length / 2).ToArray();
            int[] secondArray = initialArray.Skip(initialArray.Length / 2).ToArray();

            int[] firstArraySorted = Sort(firstArray, descending);
            int[] secondArraySorted = Sort(secondArray, descending);

            int[] resultArray = new int[initialArray.Length];

            int i = 0;
            int j = 0;
            for (int k = 0; k < resultArray.Length; k++)
            {
                if (i == firstArray.Length)
                {
                    resultArray[k] = secondArraySorted[j];
                    j++;
                    continue;
                }

                if (j == secondArray.Length)
                {
                    resultArray[k] = firstArraySorted[i];
                    i++;
                    continue;
                }

                if ((firstArraySorted[i] > secondArraySorted[j] && !descending)
                    || (firstArraySorted[i] < secondArraySorted[j] && descending))
                {
                    resultArray[k] = secondArraySorted[j];
                    j++;
                }
                else
                {
                    resultArray[k] = firstArraySorted[i];
                    i++;
                }
            }

            return resultArray;
        }

        /// <summary>
        /// This algorithm is based on a merge sort algorithm.
        /// </summary>
        /// <param name="initialArray"></param>
        /// <returns></returns>
        public static CountedInversionsIntArray CountInversionsAndSort(CountedInversionsIntArray initialArray)
        {
            if (initialArray.Length < 2)
                return initialArray;

            CountedInversionsIntArray firstArray = new CountedInversionsIntArray
            {
                Array = initialArray.Array.Take(initialArray.Length / 2).ToArray(),
                NumberOfInversions = 0
            };

            CountedInversionsIntArray secondArray = new CountedInversionsIntArray
            {
                Array = initialArray.Array.Skip(initialArray.Length / 2).ToArray(),
                NumberOfInversions = 0
            };

            CountedInversionsIntArray firstArraySorted = CountInversionsAndSort(firstArray);
            CountedInversionsIntArray secondArraySorted = CountInversionsAndSort(secondArray);

            CountedInversionsIntArray resultArray = new CountedInversionsIntArray
            {
                Array = new int[initialArray.Length],
                NumberOfInversions = firstArraySorted.NumberOfInversions + secondArraySorted.NumberOfInversions // only left and right inversions are counted here
            };                                                                                                 // now we also need split inversions

            int numberOfSplitInversions = 0;
            int i = 0;
            int j = 0;
            for (int k = 0; k < resultArray.Length; k++)
            {
                if (i == firstArray.Length)
                {
                    resultArray[k] = secondArraySorted[j];
                    j++;
                    continue;
                }

                if (j == secondArray.Length)
                {
                    resultArray[k] = firstArraySorted[i];
                    i++;
                    continue;
                }

                if (firstArraySorted[i] > secondArraySorted[j])
                {
                    resultArray[k] = secondArraySorted[j];
                    for (int l = i; l < firstArraySorted.Length; l++)
                    {
                        //Console.WriteLine("Inverstion found ({0},{1})", firstArraySorted[l], secondArraySorted[j]);
                        numberOfSplitInversions++;
                    }
                    j++;
                }
                else
                {
                    resultArray[k] = firstArraySorted[i];
                    i++;
                }
            }

            resultArray.NumberOfInversions += numberOfSplitInversions;
            return resultArray;
        }
    }
}
