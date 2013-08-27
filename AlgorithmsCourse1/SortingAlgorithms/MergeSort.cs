using System.Linq;

namespace AlgorithmsCourse1.SortingAlgorithms
{
    class MergeSort
    {
        public int[] Sort(int[] initialArray, bool descending)
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
    }
}
