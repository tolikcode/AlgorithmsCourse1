using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsCourse1.TasksImplementations
{
    internal class SccVertex
    {
        public SccVertex()
        {
            InVertices = new List<SccVertex>();
            OutVertices = new List<SccVertex>();
            LeadVertices = new List<SccVertex>();
        }

        public List<SccVertex> InVertices { get; set; }
        public List<SccVertex> OutVertices { get; set; }
        public bool IsExplored { get; set; }
        public List<SccVertex> LeadVertices { get; set; }
    }

    /// <summary>
    /// Algorithm for finding strongly connected components in a directed graph.
    /// </summary>
    class StronglyConnectedComponents
    {
        private SccVertex currentLeaderVertex;
        private List<SccVertex> finishedOrderedGraph;
 
        /// <summary>
        /// Computes all SCC and prints to screen n largest.
        /// </summary>
        /// <param name="graph">Graph vertices</param>
        /// <param name="numberOfComponents">Nymber of largest SCC to find</param>
        /// <returns>SCC leader nodes</returns>
        public SccVertex[] ComputeLargestScc(SccVertex[] graph, int numberOfComponents)
        {
            currentLeaderVertex = null;
            finishedOrderedGraph = new List<SccVertex>();

            // First stage. Going backwards in graph and determining finishing order (corresponds to an order in finishedOrderedGraph)
            for (int i = 1; i < graph.Length; i++)
            {
                if(!graph[i].IsExplored)
                    DepthFirstSearch(graph[i], true);
            }

            Console.WriteLine("Finishing order for all vertices is determined.");

            foreach (SccVertex sccVertex in finishedOrderedGraph)
            {
                sccVertex.IsExplored = false;
            }

            // Second stage. Determining SCCs (in a SCC all vertices have the same leader vertex)
            for (int i = finishedOrderedGraph.Count - 1; i >= 0; i--)
            {
                if (!finishedOrderedGraph[i].IsExplored)
                {
                    currentLeaderVertex = finishedOrderedGraph[i];
                    DepthFirstSearch(finishedOrderedGraph[i], false);
                }
            }

            SccVertex[] sccLeaderNodes = finishedOrderedGraph.Where(v => v.LeadVertices.Count > 0)
                                            .OrderByDescending(v => v.LeadVertices.Count)
                                            .ToArray();

            Console.WriteLine("All SCCs were found. {0} largest SCC:", numberOfComponents);
            for (int i = 0; i < numberOfComponents; i++)
            {
                Console.WriteLine(sccLeaderNodes[i].LeadVertices.Count);
            }

            return sccLeaderNodes;
        }

        private void DepthFirstSearch(SccVertex startVertex, bool reverseMode)
        {
            startVertex.IsExplored = true;
            if(!reverseMode)
                currentLeaderVertex.LeadVertices.Add(startVertex);
            foreach (SccVertex sccVertex in reverseMode ? startVertex.InVertices : startVertex.OutVertices)
            {
                if(!sccVertex.IsExplored)
                    DepthFirstSearch(sccVertex, reverseMode);
            }
            if(reverseMode)
                finishedOrderedGraph.Add(startVertex);
        }
    }
}
