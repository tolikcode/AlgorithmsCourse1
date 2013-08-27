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
            ProgrammingQuestion5();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static void ProgrammingQuestion1()
        {
            int[] initialArray = new int[100000];
            StreamReader streamReader = new StreamReader(@"TasksData\IntegerArray (Task1).txt");

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

        private static void ProgrammingQuestion2()
        {
            int[] initialArray = new int[10000];
            StreamReader streamReader = new StreamReader(@"TasksData\QuickSort (Task2).txt");

            string line;
            int i = 0;
            while ((line = streamReader.ReadLine()) != null)
            {
                initialArray[i] = int.Parse(line);
                i++;
            }

            QuickSortComparisonsCount comparisonsCount = new QuickSortComparisonsCount();

            int numberOfComparisons = comparisonsCount.CountNumberOfComparisons(initialArray, PivotSelectionMethod.MedianOfThree);

            Console.WriteLine("Number of comparisons " + numberOfComparisons);
        }

        private static void ProgrammingQuestion3()
        {
            StreamReader streamReader = new StreamReader(@"TasksData\kargerMinCut (Task3).txt");
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

        private static void ProgrammingQuestion4()
        {
            int numberOfVertices = 875714; // known from a file

            SccVertex[] graph = new SccVertex[numberOfVertices + 1]; // position 0 in the array will always be empty
            for (int i = 1; i <= numberOfVertices; i++)
            {
                graph[i] = new SccVertex();
            }

            StreamReader streamReader = new StreamReader(@"TasksData\SCC (Task4).txt");
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

            StrongComponents sccAlgorithm = new StrongComponents();
            Thread thread = new Thread(() => sccAlgorithm.Compute(graph), 20 * 1024 * 1024); // using an increased stack because of deep recursion
                                                                                             // another approach would be to implement DFS using a stack data structure
                                                                                             // instead of a recursion
            thread.Start();
        }

        private static void ProgrammingQuestion5()
        {
            string[] taskLines = File.ReadAllLines(@"TasksData\dijkstraData (Task5).txt");
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

    }
}
