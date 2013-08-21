using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse1
{
    internal struct Edge
    {
        readonly int vertex1;
        readonly int vertex2;

        public Edge(int vertex1, int vertex2)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }

        public int Vertex1
        {
            get { return vertex1; }
        }

        public int Vertex2
        {
            get { return vertex2; }
        }
    }


    class KargersContraction
    {
        public int FindMinCut(List<Edge> graph)
        {
            Random random = new Random();

            while (GraphHasMoreThanTwoVertices(graph))
            {
                Edge edgeToContract = graph[random.Next(0, graph.Count())];

                //now we take all edges connected to edgeToContract.Vertex2 and connect them to edgeToContract.Vertex1 instead
                for (int i = 0; i < graph.Count(); i++)
                {
                    if (graph[i].Vertex1 == edgeToContract.Vertex2)
                    {
                        if (graph[i].Vertex2 == edgeToContract.Vertex1)
                        {
                            graph.RemoveAt(i); // deleting parallel edge
                            i--;
                        }
                        else
                        {
                            graph[i] = new Edge(edgeToContract.Vertex1, graph[i].Vertex2);
                        }
                    }
                    else if (graph[i].Vertex2 == edgeToContract.Vertex2)
                    {
                        if (graph[i].Vertex1 == edgeToContract.Vertex1)
                        {
                            graph.RemoveAt(i); // deleting parallel edge
                            i--;
                        }
                        else
                        {
                            graph[i] = new Edge(graph[i].Vertex1, edgeToContract.Vertex1);
                        }
                    }
                }

            }

            return graph.Count();
        }

        private bool GraphHasMoreThanTwoVertices(List<Edge> graph)
        {
            if (graph.Count < 2)
                return false;

            int vertex1 = graph[0].Vertex1;
            int vertex2 = graph[0].Vertex2;

            for (int i = 1; i < graph.Count; i++)
            {
                if (graph[i].Vertex1 != vertex1 && graph[i].Vertex1 != vertex2)
                    return true;

                if (graph[i].Vertex2 != vertex1 && graph[i].Vertex2 != vertex2)
                    return true;
            }

            return false;
        }

    }
}
