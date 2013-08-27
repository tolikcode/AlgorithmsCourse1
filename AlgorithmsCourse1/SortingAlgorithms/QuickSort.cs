using System;
using System.Linq;

namespace AlgorithmsCourse1.SortingAlgorithms
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
        public static T[] Sort<T>(T[] array, int startIndex, int endIndex, PivotSelectionMethod pivotSelection) where T : IComparable<T>
        {
            if(startIndex > endIndex)
                throw new ArgumentException("Start index can't be bigger than end index.");

            if (startIndex == endIndex)
                return array;

            int pivotElementIndex = GetPivotIndex(array, startIndex, endIndex, pivotSelection);
            T pivotValue = array[pivotElementIndex];
            Swap(array, startIndex, pivotElementIndex);

            int firstBiggerIndex = startIndex + 1; // index of the first element that is bigger than pivot element
            for (int j = startIndex + 1; j <= endIndex; j++)
            {
                if (array[j].CompareTo(pivotValue) == -1) // there might be a redundant swap if the first element we check is less than pivot,
                {                                         // but this won't hurt anybody
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

        private static int GetPivotIndex<T>(T[] array, int startIndex, int endIndex, PivotSelectionMethod pivotSelection) where T : IComparable<T>
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

                    Tuple<int, T>[] keyElements = new Tuple<int, T>[3];
                    keyElements[0] = new Tuple<int, T>(startIndex, array[startIndex]);
                    keyElements[1] = new Tuple<int, T>(middleIndex, array[middleIndex]);
                    keyElements[2] = new Tuple<int, T>(endIndex, array[endIndex]);
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

        private static void Swap<T>(T[] array, int index1, int index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }

    }
}
