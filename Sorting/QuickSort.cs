using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse1
{
    internal enum PivotSelectionMethod
    {
        FirstElement,
        LastElement,
        MedianOfThree,
        Random
    }

    class QuickSort
    {
        private int numberOfComparisons;

        public int CountNumberOfComparisons(int[] array, PivotSelectionMethod pivotSelection)
        {
            numberOfComparisons = 0;

            Sort(array, 0, array.Length - 1, pivotSelection);

            return numberOfComparisons;
        }

        public int[] Sort(int[] array, int startIndex, int endIndex, PivotSelectionMethod pivotSelection)
        {
            if(startIndex > endIndex)
                throw new ArgumentException("Start index can't be bigger than end index.");

            if (startIndex == endIndex)
                return array;

            numberOfComparisons += endIndex - startIndex;

            int pivotElementIndex = GetPivotIndex(array, startIndex, endIndex, pivotSelection);
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
                Sort(array, startIndex, pivotIndex - 1, pivotSelection);
            if (pivotIndex != endIndex)
                Sort(array, pivotIndex + 1, endIndex, pivotSelection);

            return array;
        }

        private int GetPivotIndex(int[] array, int startIndex, int endIndex, PivotSelectionMethod pivotSelection)
        {
            switch (pivotSelection)
            {
                case PivotSelectionMethod.FirstElement:
                {
                    return startIndex;
                }
                case PivotSelectionMethod.LastElement:
                {
                    return endIndex;
                }
                case PivotSelectionMethod.MedianOfThree: // the median (by value) of three elements (first, middle, last)
                {
                    if (endIndex - startIndex < 2)
                        return startIndex; //pivot index in this case won't influence the number of comparisons

                    int arrayLength = endIndex - startIndex + 1;
                    int middleIndex = startIndex + arrayLength/2;
                    if (arrayLength%2 == 0)
                        middleIndex--;

                    Tuple<int, int>[] keyElements = new Tuple<int, int>[3];
                    keyElements[0] = new Tuple<int, int>(startIndex, array[startIndex]);
                    keyElements[1] = new Tuple<int, int>(middleIndex, array[middleIndex]);
                    keyElements[2] = new Tuple<int, int>(endIndex, array[endIndex]);
                    keyElements = keyElements.OrderBy(el => el.Item2).ToArray();

                    return keyElements[1].Item1;
                }
                case PivotSelectionMethod.Random:
                {
                    Random random = new Random();
                    return random.Next(startIndex, endIndex + 1); // endIndex + 1 is because upper bound (max value) is exclusive
                }
                default:
                    throw new ArgumentOutOfRangeException("Unknown pivot selection method");
            }
        }

        private void Swap(int[] array, int index1, int index2)
        {
            int temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }

    }
}
