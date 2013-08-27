using System;
using System.Linq;

namespace AlgorithmsCourse1.SortingAlgorithms
{
    class MergeSort
    {
        public static T[] Sort<T>(T[] initialArray, bool descending) where T : IComparable<T>
        {
            if (initialArray.Length < 2)
                return initialArray;

            T[] firstArray = initialArray.Take(initialArray.Length / 2).ToArray();
            T[] secondArray = initialArray.Skip(initialArray.Length / 2).ToArray();

            T[] firstArraySorted = Sort(firstArray, descending);
            T[] secondArraySorted = Sort(secondArray, descending);

            T[] resultArray = new T[initialArray.Length];

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

                if ((firstArraySorted[i].CompareTo(secondArraySorted[j]) == 1 && !descending)
                    || (firstArraySorted[i].CompareTo(secondArraySorted[j]) == -1 && descending))
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
    }
}
