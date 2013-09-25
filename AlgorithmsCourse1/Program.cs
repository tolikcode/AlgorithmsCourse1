using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AlgorithmsCourse1.DataStructures;
using AlgorithmsCourse1.SortingAlgorithms;
using AlgorithmsCourse1.TasksImplementations;

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
            Console.WriteLine("AlgorithmsCourse1");
            Console.WriteLine("This project contains implementation of different algorithms and data structures I studied during Algorithms: Design and Analysis, Part 1 course on Coursera.");

            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        /// <summary>
        /// Computes the number of inversions in the file given, where the i-th row of the file indicates the i-th entry of an array
        /// using MergeSort like algorithm.
        /// </summary>
        private static void CountInversions()
        {
            int[] initialArray = new int[100000];
            StreamReader streamReader = new StreamReader(@"TasksData\IntegerArray.txt");

            string line;
            int i = 0;
            while ((line = streamReader.ReadLine()) != null)
            {
                initialArray[i] = int.Parse(line);
                i++;
            }

            CountedInversionsIntArray inversionsIntArray = new CountedInversionsIntArray {Array = initialArray};

            InversionsCount inversionsCount = new InversionsCount();

            CountedInversionsIntArray sortedArray = inversionsCount.CountInversionsAndSort(inversionsIntArray);

            Console.WriteLine("Number of inversions: {0}", sortedArray.NumberOfInversions);
        }

        /// <summary>
        /// Computes the total number of comparisons used to sort the given input file by QuickSort
        /// with different methods of pivot element selection.
        /// </summary>
        private static void CountComparisonsInQuickSort()
        {
            int[] initialArray = new int[10000];
            StreamReader streamReader = new StreamReader(@"TasksData\QuickSort.txt");

            string line;
            int i = 0;
            while ((line = streamReader.ReadLine()) != null)
            {
                initialArray[i] = int.Parse(line);
                i++;
            }

            QuickSortComparisonsCount comparisonsCount = new QuickSortComparisonsCount();

            int firstElementComparisons = comparisonsCount.CountNumberOfComparisons((int[])initialArray.Clone(), PivotSelectionMethod.FirstElement);
            int randomComparisons = comparisonsCount.CountNumberOfComparisons((int[])initialArray.Clone(), PivotSelectionMethod.Random);
            int medianOfThreeComparisons = comparisonsCount.CountNumberOfComparisons((int[])initialArray.Clone(), PivotSelectionMethod.MedianOfThree);

            Console.WriteLine("Number of comparisons (first element selected as pivot): " + firstElementComparisons);
            Console.WriteLine("Number of comparisons (random elemente selected as pivot): " + randomComparisons);
            Console.WriteLine("Number of comparisons (median of three selected as pivot): " + medianOfThreeComparisons);
        }

        /// <summary>
        /// Computes the min cut of a graph with with Kargers randomized contraction algorithm.
        /// 
        /// The file contains the adjacency list representation of a simple undirected graph. 
        /// The first column in the file represents the vertex label, and the particular row (other entries except the first column) tells
        /// all the vertices that the vertex is adjacent to.
        /// </summary>
        private static void FindGraphsMinCut()
        {
            StreamReader streamReader = new StreamReader(@"TasksData\kargerMinCut.txt");
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

        /// <summary>
        /// Computes all SCC and prints to screen n largest.
        /// 
        /// Input file contains the edges of a directed graph.
        /// Every row indicates an edge, the vertex label in first column is the tail and the vertex label in second column is the head.
        /// </summary>
        private static void ComputeStronglyConnectedComponents()
        {
            int numberOfVertices = 875714; // from the file
            int requiredNumberOfLargestComponents = 5; // from the task

            SccVertex[] graph = new SccVertex[numberOfVertices + 1]; // position 0 in the array will always be empty
            for (int i = 1; i <= numberOfVertices; i++)
            {
                graph[i] = new SccVertex();
            }

            StreamReader streamReader = new StreamReader(@"TasksData\SCC.txt");
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                string[] edgeVertices = line.Split(' ');
                SccVertex taleVertex = graph[int.Parse(edgeVertices[0])];
                SccVertex headVertex = graph[int.Parse(edgeVertices[1])];
                taleVertex.OutVertices.Add(headVertex);
                headVertex.InVertices.Add(taleVertex);
            }
            Console.WriteLine("Reading of a graph from a file is finished.");
            

            StronglyConnectedComponents sccAlgorithm = new StronglyConnectedComponents();
            Thread thread = new Thread(
                () => sccAlgorithm.ComputeLargestScc(graph, requiredNumberOfLargestComponents), 20 * 1024 * 1024); // using an increased stack because of deep recursion
                                                                                                                   // another approach would be to implement DFS using a stack data structure
                                                                                                                   // instead of a recursion
            thread.Start();
        }

        /// <summary>
        /// Computes shortest-path distances between start vertex and every other vertex of the undirected graph (using Dijkstra algorithm),
        /// and prints out distances to target vertices.
        /// 
        /// The file contains an adjacency list representation of an undirected weighted graph. Each row consists of the node tuples
        /// that are adjacent to that particular vertex along with the length of that edge.
        /// </summary>
        private static void RunDijkstraAlgorithm()
        {
            string[] taskLines = File.ReadAllLines(@"TasksData\dijkstraData.txt");
            int numberOfVertices = taskLines.Count();

            DijkstraVertex[] verticesArray = new DijkstraVertex[numberOfVertices + 1]; // position 0 in the array will always be empty
            for (int i = 1; i <= numberOfVertices; i++)
            {
                verticesArray[i] = new DijkstraVertex(i) { ShortestPath = 1000000 }; // 1000000 - from the task
            }

            foreach (string taskLine in taskLines)
            {
                string[] vertexEdges = taskLine.Split('\t');

                DijkstraVertex currentVertex = verticesArray[int.Parse(vertexEdges[0])];

                for (int i = 1; i < vertexEdges.Length - 1; i++) // vertexEdges.Length - 1 because the last element is empty just before line end
                {
                    string[] vertexEdge = vertexEdges[i].Split(',');
                    DijkstraVertex otherVertex = verticesArray[int.Parse(vertexEdge[0])];
                    int otherVertexDistance = int.Parse(vertexEdge[1]);
                    currentVertex.NeighborVertices.Add(new Tuple<DijkstraVertex, int>(otherVertex, otherVertexDistance));
                }
            }

            DijkstraAlgorithm dijkstraAlgorithm = new DijkstraAlgorithm();
            dijkstraAlgorithm.Compute(verticesArray, 1);

            int[] targetVertices = new int[] { 7, 37, 59, 82, 99, 115, 133, 165, 188, 197 };
            Console.WriteLine("Shortest paths to target vertices:");
            foreach (int targetVertex in targetVertices)
            {
                Console.Write(verticesArray[targetVertex].ShortestPath + ",");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// A variant of the 2-SUM algorithm. 
        /// Computes the number of target values t in the interval [-10000,10000] (inclusive) such that there are distinct numbers x,y in the input file that satisfy x+y=t.
        /// 
        /// Input file contains 1 million integers, both positive and negative (there might be some repetitions).
        /// </summary>
        private static void TwoSumAlgorithm()
        {
            string[] taskLines = File.ReadAllLines(@"TasksData\algo1-programming_prob-2sum.txt");
            long[] integers = taskLines.Select(taskLine => long.Parse(taskLine)).ToArray();

            TwoSum twoSum = new TwoSum();
            int numberOfTwoSums = twoSum.Calculate(integers, -10000, 10000); // [-10000, 10000] range from the task
            Console.WriteLine("Number of 2-Sums " + numberOfTwoSums);
        }

        /// <summary>
        /// Determines a mediane after every next integer in an array, and then sums all medianes.
        /// Result is sum of these 10000 medians, modulo 10000 (i.e., only the last 4 digits).
        /// 
        ///  Input file contains a list of the integers from 1 to 10000 in unsorted order; you should treat this as a stream of numbers.
        /// </summary>
        private static void MedianMaintenance()
        {
            int[] integersForMedian = File.ReadAllLines(@"TasksData\Median.txt").Select(s => int.Parse(s)).ToArray();

            SumOfMedians sumOfMedians = new SumOfMedians();
            int mediansSum = sumOfMedians.Calculate(integersForMedian);
            Console.WriteLine("Sum of medians = " + mediansSum);
            Console.WriteLine("Sum of medians mod 10000 = " + (mediansSum - (mediansSum / 10000) * 10000));
        }

    }
}
