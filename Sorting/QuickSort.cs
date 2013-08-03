using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse1
{
    class QuickSort
    {
        public static int[] Sort(int[] array, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
                return array;

            Random random = new Random();
            int pivotElementIndex = random.Next(startIndex, endIndex + 1); // endIndex + 1 is because upper bound (max value) is exclusive

            int pivotValue = array[pivotElementIndex];
            Swap(array, startIndex, pivotElementIndex);

            int firstBiggerIndex = startIndex + 1; // index of the first element that is bigger than pivot element
            for (int j = startIndex + 1; j <= endIndex; j++)
            {
                if (array[j] < pivotValue) // there might be a redundant swap if the first element we check is less than pivot,
                {                          // but this won't hurt anybody
                    Swap(array, firstBiggerIndex, j);
                    firstBiggerIndex++;
                }
            }
            int pivotIndex = firstBiggerIndex - 1;
            Swap(array, startIndex, pivotIndex);
            if (pivotIndex != startIndex)
                Sort(array, startIndex, pivotIndex - 1);
            if (pivotIndex != endIndex)
                Sort(array, pivotIndex + 1, endIndex);

            return array;
        }

        private static void Swap(int[] array, int index1, int index2)
        {
            int temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }

    }
}
