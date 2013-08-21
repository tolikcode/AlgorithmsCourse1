using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse1
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
    /// Dijkstra algorithm for undirected graph.
    /// </summary>
    class DijkstraAlgorithm
    {
        public DijkstraVertex[] Compute(DijkstraVertex[] verticesArray)
        {


            return verticesArray;
        }
    }
}
