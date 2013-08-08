﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ProgrammingQuestion3();

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

            MergeSort mergeSort = new MergeSort();

            CountedInversionsIntArray sortedArray = mergeSort.CountInversionsAndSort(inversionsIntArray);

            Console.WriteLine("Number of inversions: {0}", sortedArray.NumberOfInversions);
        }

        private static void ProgrammingQuestion2()
        {
            int[] initialArray = new int[10000];
            StreamReader streamReader = new StreamReader(@"D:\QuickSort.txt");

            string line;
            int i = 0;
            while ((line = streamReader.ReadLine()) != null)
            {
                initialArray[i] = int.Parse(line);
                i++;
            }

            QuickSort quickSort = new QuickSort();

            int numberOfComparisons = quickSort.CountNumberOfComparisons(initialArray, PivotSelectionMethod.MedianOfThree);

            Console.WriteLine(numberOfComparisons);
        }

        private static void ProgrammingQuestion3()
        {
            StreamReader streamReader = new StreamReader(@"D:\kargerMinCut.txt");
            List<Edge> graph = new List<Edge>();

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                string[] lineNumbers = line.Split('\t');

                int vertexNumber = int.Parse(lineNumbers[0]);

                for (int i = 1; i < lineNumbers.Length - 1; i++) // lineNumbers.Length - 1 because the last element is empty just before line end
                {
                    int otherVertexNumber = int.Parse(lineNumbers[i]);

                    if (otherVertexNumber < vertexNumber)
                        continue; // if other vertex number is smaller then we must have added this edge already

                    graph.Add(new Edge(vertexNumber, otherVertexNumber));
                }
            }

            KargersContraction kargersContraction = new KargersContraction();

            int absoluteMinCut = graph.Count;
            Object thisLock = new Object();

            int numberOfTrials = 212000; // n = 200 (number of vertices)
                                         // with number of trials = (n^2)*ln(n) the probability of failure is 1/n
                                         // Remark: This is too much. It usually finds answer in a first few hundreads of iterations
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Parallel.For(0, numberOfTrials, i =>
                {
                    int currentMinCut = kargersContraction.FindMinCut(new List<Edge>(graph));
                    lock (thisLock)
                    {
                        if (currentMinCut < absoluteMinCut)
                        {
                            absoluteMinCut = currentMinCut;
                            Console.WriteLine("!!! New min cut found: " + absoluteMinCut);
                        }
                    }
                    if (i%1000 == 0)
                    {
                        Console.WriteLine("Working around index: " + i);
                        Console.WriteLine("Time elapsed: " + stopwatch.Elapsed); // even though Stopwatch is not guaranteed to be threadsafe,
                    }                                                            // I hope it'll be OK with 64 bit processor
                    
                });

            stopwatch.Stop();
            Console.WriteLine("The min cut is: " + absoluteMinCut);
            Console.WriteLine("Total time elapsed: " + stopwatch.Elapsed);
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
    }
}
