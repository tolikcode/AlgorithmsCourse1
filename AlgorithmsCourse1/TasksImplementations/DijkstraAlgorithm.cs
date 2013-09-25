using System;
using System.Collections.Generic;
using AlgorithmsCourse1.DataStructures;

namespace AlgorithmsCourse1.TasksImplementations
{
    internal class DijkstraVertex : IComparable<DijkstraVertex>
    {
        public DijkstraVertex(int vertexNumber)
        {
            this.VertexNumber = vertexNumber;
            NeighborVertices = new List<Tuple<DijkstraVertex, int>>();
        }

        public List<Tuple<DijkstraVertex, int>> NeighborVertices { get; private set; }
        public int VertexNumber { get; private set; }
        public int ShortestPath { get; set; }

        public int CompareTo(DijkstraVertex other)
        {
            return this.ShortestPath.CompareTo(other.ShortestPath);
        }
    }

    /// <summary>
    /// Dijkstra algorithm for computing the shortest-path distances between start vertex and every other vertex of the undirected graph.
    /// Heap-based implementation.
    /// </summary>
    class DijkstraAlgorithm
    {
        public DijkstraVertex[] Compute(DijkstraVertex[] verticesArray, int startVertexIndex)
        {
            verticesArray[startVertexIndex].ShortestPath = 0;
            foreach (Tuple<DijkstraVertex, int> vertex in verticesArray[startVertexIndex].NeighborVertices)
            {
                vertex.Item1.ShortestPath = vertex.Item2;
            }

            Heap<DijkstraVertex> minHeap = new Heap<DijkstraVertex>(false);

            // they say it's possible to do bulk insert into heap in O(n),
            // but I consider O(nlogn) good enough for this task
            for (int i = 1; i < verticesArray.Length; i++)
            {
                minHeap.Insert(verticesArray[i]); 
            }

            DijkstraVertex currentMinVertex;
            while (minHeap.Count != 0)
            {
                currentMinVertex = minHeap.ExtractRoot();
                foreach (Tuple<DijkstraVertex, int> neighborVertex in currentMinVertex.NeighborVertices)
                {
                    if (minHeap.Contains(neighborVertex.Item1))
                    {
                        minHeap.Delete(neighborVertex.Item1);

                        int newPathDistance = currentMinVertex.ShortestPath + neighborVertex.Item2;
                        if(newPathDistance < neighborVertex.Item1.ShortestPath)
                            neighborVertex.Item1.ShortestPath = newPathDistance;

                        minHeap.Insert(neighborVertex.Item1);
                    }
                }
            }

            return verticesArray;
        }
    }
}
