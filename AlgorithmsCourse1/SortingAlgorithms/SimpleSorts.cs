using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse1.SortingAlgorithms
{
    class SimpleSorts
    {
        public static T[] BubbleSort<T>(T[] array) where T : IComparable<T>
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j].CompareTo(array[j + 1]) == 1)
                    {
                        Swap(array, j, j + 1);
                    }
                }
            }

            return array;
        }

        public static T[] SelectionSort<T>(T[] array) where T : IComparable<T>
        {
            for (int i = 0; i < array.Length; i++)
            {
                int indexOfCurrentSmallest = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j].CompareTo(array[indexOfCurrentSmallest]) == -1)
                        indexOfCurrentSmallest = j;
                }
                Swap(array, i, indexOfCurrentSmallest);
            }

            return array;
        }

        private static void Swap<T>(T[] array, int index1, int index2)
        {
            if (index1 == index2)
                return;

            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }

    }
}
