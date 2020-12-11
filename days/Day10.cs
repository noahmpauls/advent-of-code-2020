using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace days
{
    public class Day10
    {
        // TODO: generalize more

        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static long Part1()
        {
            const string path = Helpers.inputPath + @"\day10\input.txt";
            List<int> inputs = ProcessInputFile(path).ToList();
            inputs.Add(0);
            inputs.Add(inputs.Max() + 3);
            inputs.Sort();
            Counter<int> differences = new Counter<int>();
            for (int i = 1; i < inputs.Count; i++)
            {
                differences.Add(inputs[i] - inputs[i - 1]);
            }
            return differences.Count(1) * differences.Count(3);
        }

        public static long Part2()
        {
            const string path = Helpers.inputPath + @"\day10\input.txt";
            List<int> inputs = ProcessInputFile(path).ToList();
            int source = 0;
            int target = inputs.Max() + 3;
            inputs.Add(source);
            inputs.Add(target);
            IDictionary<int, ISet<int>> graph = BackwardNeighborGraph(inputs, 1, 3);
            long pathCount = CountPathsBetweenDAG(graph, source, target);
            return pathCount;
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IList<int> ProcessInputFile(string path)
        {
            return Helpers.ProcessInputFile(path, line => int.Parse(line));
        }

        // Version where all paths are returned is crazy expensive on large graphs
        // Only works on directed acyclic graphs
        public static long CountPathsBetweenRec<T>(IDictionary<T, ISet<T>> graph, T source, T target)
        {
            long pathCount = 0;
            foreach(T adjacent in graph[source])
            {
                if (adjacent.Equals(target)) pathCount++;
                else pathCount += CountPathsBetweenRec(graph, adjacent, target);
            }
            return pathCount;
        }

        /* This method counts paths by assigning a "paths into" count for every
         *  node in the graph. The source has one path going into it; from there,
         *  the paths into each node is the sum of the "paths into" values for
         *  all other nodes with edges leading into the given node.
         */
        public static long CountPathsBetweenDAG<T>(IDictionary<T, ISet<T>> graph, T source, T target)
        {
            Counter<T> pathsInto = new Counter<T>();
            List<T> orderedNodes = graph.Keys.ToList();
            orderedNodes.Sort();

            pathsInto.Add(source, 1);
            for (int i = orderedNodes.IndexOf(source) + 1; i < orderedNodes.Count; i++)
            {
                T node = orderedNodes[i];
                foreach (T adjacent in graph[node])
                {
                    pathsInto.Add(node, pathsInto.Count(adjacent));
                }
                if (node.Equals(target)) break;
            }
            return pathsInto.Count(target);
        }

        /* Create a graph of integers where there is a directed edge from a to
         *  b if indexA > indexB and dl <=|a-b| <= dh. dl >= 1 and dh >= dl.
         */
        public static IDictionary<int, ISet<int>> ForwardNeighborGraph(IList<int> nodes, int dl, int dh)
        {
            IDictionary<int, ISet<int>> graph = new Dictionary<int, ISet<int>>();
            List<int> nodesSorted = new List<int>(nodes);
            nodesSorted.Sort();

            for (int i = 0; i < nodesSorted.Count; i++)
            {
                graph[nodesSorted[i]] = new HashSet<int>();
                for (int j = i+1; j < nodesSorted.Count; j++)
                {
                    int difference = nodesSorted[j] - nodesSorted[i];
                    if (difference >= dl && difference <= dh)
                    {
                        graph[nodesSorted[i]].Add(nodesSorted[j]);
                    }
                    else break;
                }
            }
            return graph;
        }

        /* Create a graph of integers where there is a directed edge from a to
         *  b if indexA < indexB and dl <=|a-b| <= dh. dl >= 1 and dh >= dl.
         */
        public static IDictionary<int, ISet<int>> BackwardNeighborGraph(IList<int> nodes, int dl, int dh)
        {
            IDictionary<int, ISet<int>> graph = new Dictionary<int, ISet<int>>();
            List<int> nodesSorted = new List<int>(nodes);
            nodesSorted.Sort();
            nodesSorted.Reverse();

            for (int i = 0; i < nodesSorted.Count; i++)
            {
                graph[nodesSorted[i]] = new HashSet<int>();
                for (int j = i + 1; j < nodesSorted.Count; j++)
                {
                    int difference = Math.Abs(nodesSorted[j] - nodesSorted[i]);
                    if (difference >= dl && difference <= dh)
                    {
                        graph[nodesSorted[i]].Add(nodesSorted[j]);
                    }
                    else break;
                }
            }
            return graph;
        }

    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

}

