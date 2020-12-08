using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace days
{
    public class Day07
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static int Part1()
        {
            const string path = Helpers.inputPath + @"\day07\input.txt";
            IList<(string, IList<(string, int)>)> inputs = ProcessInputFile(path);
            BagTree bagTree = new BagTree(inputs);
            return bagTree.BagsContaining("shiny gold").Count;
        }

        public static int Part2()
        {
            const string path = Helpers.inputPath + @"\day07\input.txt";
            IList<(string, IList<(string, int)>)> inputs = ProcessInputFile(path);
            BagTree bagTree = new BagTree(inputs);
            return bagTree.TotalBags("shiny gold") - 1;
        }

        //######################################################################
        // Parsing
        //######################################################################

        // TODO: parse into dictionary instead of List of tuples
        public static IList<(string, IList<(string, int)>)> ProcessInputFile(string path)
        {
            return Helpers.ProcessInputFile(path, ExtractBagRelations);
        }

        public static (string, IList<(string, int)>) ExtractBagRelations(string line)
        {
            Regex rxPair = new Regex("(?<parentbag>.+) bags contain (?<childbags>.*)\\.");
            Match pairMatch = rxPair.Match(line);
            string parentBag = pairMatch.Groups["parentbag"].Value;

            string childString = pairMatch.Groups["childbags"].Value;
            Regex rxChildren = new Regex("no other bags|((?<quantity>[0-9]+) (?<color>[a-z ]+) bags?)");
            MatchCollection childMatches = rxChildren.Matches(childString);

            IList<(string, int)> children = new List<(string, int)>();
            foreach (Match m in childMatches)
            {
                if (m.Value.Equals("no other bags")) continue;
                string childColor = m.Groups["color"].Value;
                int childQuantity = int.Parse(m.Groups["quantity"].Value);
                children.Add((childColor, childQuantity));
            }

            return (parentBag, children);
        }

        //######################################################################
        // Methods
        //######################################################################

    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

    public class BagTree
    {
        private ISet<string> colors = new HashSet<string>();
        private IDictionary<string, ISet<string>> colorChildren = new Dictionary<string, ISet<string>>();
        private IDictionary<string, ISet<string>> colorParents = new Dictionary<string, ISet<string>>();
        private IDictionary<(string, string), int> relationQuantities = new Dictionary<(string, string), int>();

        public BagTree(IList<(string, IList<(string, int)>)> bagRelations)
        {
            foreach(var relation in bagRelations)
            {
                string parent = relation.Item1;
                colors.Add(parent);
                // TODO: figure out why both of these are necessary
                if (!colorParents.ContainsKey(parent)) colorParents[parent] = new HashSet<string>();
                if (!colorChildren.ContainsKey(parent)) colorChildren[parent] = new HashSet<string>();
                foreach (var child in relation.Item2)
                {
                    string childColor = child.Item1;
                    int childQuantity = child.Item2;
                    // add parent to child relation
                    if (!colorChildren.ContainsKey(childColor)) colorChildren[childColor] = new HashSet<string>();
                    colorChildren[parent].Add(childColor);

                    // add child to parent relation
                    if (!colorParents.ContainsKey(childColor)) colorParents[childColor] = new HashSet<string>();
                    colorParents[childColor].Add(parent);

                    // add quantity relation
                    relationQuantities[(parent, childColor)] = childQuantity;
                }
            }
        }

        // find all possible ancestor bags of the given bag
        public IList<string> BagsContaining(string bag)
        {
            List<string> result = DFS(bag, colorParents).ToList();
            result.Remove(bag);
            result.Sort();
            return result;
        }

        public IList<string> BagsContained(string bag)
        {
            throw new NotImplementedException();
        }

        // TODO: how to generalize this? perhaps a generic Graph class?
        public int TotalBags(string bag)
        {
            int totalBags = 1;
            foreach (string child in colorChildren[bag])
            {
                totalBags += (relationQuantities[(bag, child)] * TotalBags(child));
            }
            return totalBags;
        }

        private static ISet<T> DFS<T>(T source, IDictionary<T, ISet<T>> graph)
        {
            ISet<T> visited = new HashSet<T>();
            Stack<T> remaining = new Stack<T>();
            remaining.Push(source);
            while (remaining.Count > 0)
            {
                T node = remaining.Pop();
                if (!visited.Contains(node))
                {
                    visited.Add(node);
                    foreach (T adjacent in graph[node])
                    {
                        remaining.Push(adjacent);
                    }
                }
            }
            return visited;
        }

        private static IList<ISet<T>> BFSLevels<T>(T source, IDictionary<T, ISet<T>> graph)
        {
            ISet<T> visited = new HashSet<T>();
            IList<ISet<T>> levels = new List<ISet<T>>();
            ISet<T> currentLevel = new HashSet<T>();
            currentLevel.Add(source);
            while (currentLevel.Count > 0)
            {
                ISet<T> nextLevel = new HashSet<T>();
                foreach (T node in currentLevel)
                {
                    foreach (T child in graph[node])
                    {
                        if (!visited.Contains(child))
                        {
                            nextLevel.Add(child);
                            visited.Add(child);
                        }
                    }
                }
                levels.Add(currentLevel);
                currentLevel = nextLevel;
            }
            return levels;
        }
    }

}