using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse1
{
    class Program
    {
        #region Help methods

        private static int[] GetShuffledIntArray(int size, bool distinct)
        {
            if (size < 1)
                throw new ArgumentException("Array size must be more than 0");

            int[] array = new int[size];
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                array[i] = distinct ? i : random.Next(0, size);
            }

            array = array.OrderBy(a => Guid.NewGuid()).ToArray();

            return array;
        }

        private static bool CheckSorted(int[] initialArray, int[] sortedArray, bool descending)
        {
            if (initialArray.Length != sortedArray.Length)
                throw new ArgumentException("Sorted array must be of the same size as initial one.");

            int[] modelArray =
                (descending ? initialArray.OrderByDescending(a => a) : initialArray.OrderBy(a => a)).ToArray();

            for (int i = 0; i < sortedArray.Length; i++)
            {
                if (sortedArray[i] != modelArray[i])
                    return false;
            }

            return true;
        }

        #endregion Help methods

        static void Main(string[] args)
        {


            Console.WriteLine("Done.");
            Console.ReadLine();

        }

        private static void ProgrammingQuestion1()
        {
            int[] initialArray = new int[100000]; 
            StreamReader streamReader = new StreamReader(@"D:\IntegerArray.txt");

            string line;
            int i = 0;
            while ((line = streamReader.ReadLine()) != null)
            {
                initialArray[i] = int.Parse(line);
                i++;
            }

            CountedInversionsIntArray inversionsIntArray = new CountedInversionsIntArray {Array = initialArray};

            CountedInversionsIntArray sortedArray = CountInversionsAndSort(inversionsIntArray);

            Console.WriteLine("Number of inversions: {0}", sortedArray.NumberOfInversions);
        }

        private static int[] BubbleSort(int[] initialArray)
        {
            int temp;
            for (int i = 0; i < initialArray.Length; i++)
            {
                for (int j = 0; j < initialArray.Length -1; j++)
                {
                    if(initialArray[j] > initialArray[j+1])
                    {
                        temp = initialArray[j];
                        initialArray[j] = initialArray[j + 1];
                        initialArray[j + 1] = temp;
                    }
                }
            }

            return initialArray;
        }

        private static int[] MergeSort(int[] initialArray, bool descending)
        {
            if (initialArray.Length < 2)
                return initialArray;

            int[] firstArray = initialArray.Take(initialArray.Length/2).ToArray();
            int[] secondArray = initialArray.Skip(initialArray.Length/2).ToArray();

            int[] firstArraySorted = MergeSort(firstArray, descending);
            int[] secondArraySorted = MergeSort(secondArray, descending);

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

        private static CountedInversionsIntArray CountInversionsAndSort(CountedInversionsIntArray initialArray)
        {
            if (initialArray.Length < 2)
                return initialArray;

            CountedInversionsIntArray firstArray = new CountedInversionsIntArray
                {
                    Array = initialArray.Array.Take(initialArray.Length/2).ToArray(),
                    NumberOfInversions = 0
                };

            CountedInversionsIntArray secondArray = new CountedInversionsIntArray
            {
                Array = initialArray.Array.Skip(initialArray.Length/2).ToArray(),
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
