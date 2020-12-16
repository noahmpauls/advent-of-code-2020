using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace days
{
    public class Day16
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static int Part1()
        {
            const string path = Helpers.inputPath + @"\day16\input.txt";
            string input = ProcessInputFile(path);

            IList<TicketValidator.Rule> rules = ParseRules(input);
            TicketValidator validator = new TicketValidator();
            validator.AddRules(rules);

            IList<IList<int>> tickets = ParseNearbyTickets(input);

            int errorRate = 0;
            foreach (var ticket in tickets)
            {
                foreach (int val in ticket)
                {
                    if (!validator.ValidateValue(val))
                    {
                        errorRate += val;
                    }
                }
            }
            return errorRate;
        }

        public static long Part2()
        {
            const string path = Helpers.inputPath + @"\day16\input.txt";
            string input = ProcessInputFile(path);

            IList<TicketValidator.Rule> rules = ParseRules(input);
            TicketValidator validator = new TicketValidator();
            validator.AddRules(rules);

            IList<int> myTicket = ParseYourTicket(input);
            IList<IList<int>> nearbyTickets = ParseNearbyTickets(input);
            IList<string> fieldOrder = FindFieldOrder(validator, nearbyTickets);

            long result = 1;
            for (int i = 0; i < myTicket.Count; i++)
            {
                if (fieldOrder[i].StartsWith("departure"))
                    result *= (long)myTicket[i];
            }
            return result;
        }

        //######################################################################
        // Methods
        //######################################################################

        public static string ProcessInputFile(string path)
        {
            return Helpers.GetFileAsString(path);
        }

        public static IList<TicketValidator.Rule> ParseRules(string text)
        {
            var result = new List<TicketValidator.Rule>();
            // assumes a single name and exactly two bounds
            Regex rx = new Regex("(?<rule>[a-z ]+): (?<bound1>(?<min1>[0-9]+)-(?<max1>[0-9]+)) or (?<bound2>(?<min2>[0-9]+)-(?<max2>[0-9]+))");
            foreach (Match m in rx.Matches(text))
            {
                string name = m.Groups["rule"].Value;
                int min1 = int.Parse(m.Groups["min1"].Value);
                int max1 = int.Parse(m.Groups["max1"].Value);
                int min2 = int.Parse(m.Groups["min2"].Value);
                int max2 = int.Parse(m.Groups["max2"].Value);

                var rule = new TicketValidator.Rule { Name = name };
                rule.AddBound(min1, max1);
                rule.AddBound(min2, max2);
                result.Add(rule);
            }

            return result;
        }

        public static IList<IList<int>> ParseNearbyTickets(string text)
        {
            var result = new List<IList<int>>();

            Regex rxTicketBlock = new Regex(@"nearby tickets:\s+(?<tickets>([0-9]+[,\s]+)+)");
            string ticketBlock = rxTicketBlock.Match(text).Value;

            Regex rxTicket = new Regex(@"(([0-9]+),?)+");
            foreach(Match ticket in rxTicket.Matches(ticketBlock))
            {
                IList<int> ticketVals = ticket.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v => int.Parse(v)).ToList();
                result.Add(ticketVals);
            }

            return result;
        }

        public static IList<int> ParseYourTicket(string text)
        {
            Regex rxTicketBlock = new Regex(@"your ticket:\s+(?<ticket>([0-9]+,?)+)");
            string ticket = rxTicketBlock.Match(text).Groups["ticket"].Value;

            return ticket.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v => int.Parse(v)).ToList();
        }


        public static IList<string> FindFieldOrder(TicketValidator validator, IList<IList<int>> tickets)
        {
            IList<IList<int>> validTickets = tickets.Where(t => validator.ValidateValues(t)).ToList();
            int fieldCount = tickets[0].Count;

            var fieldIndices = new Dictionary<string, ISet<int>>();
            foreach (string field in validator.Rules)
            {
                var indices = new HashSet<int>(Enumerable.Range(0, fieldCount));
                fieldIndices.Add(field, indices);
            }

            IList<string> result = new List<string>();
            foreach (int i in Enumerable.Range(0, fieldCount))
                result.Add("NONE");

            // find which indices the field cannot be due to values breaking rules
            foreach (var ticket in validTickets)
            {
                for (int i = 0; i < fieldCount; i++)
                {
                    int val = ticket[i];
                    IList<string> invalidFields = validator.RulesBroken(val);
                    foreach (string field in invalidFields) {
                        fieldIndices[field].Remove(i);
                    }
                }
            }

            // deduce index assignments from remaining possible values
            var assignedIndices = new HashSet<int>();
            bool unassignedExists = false;
            do
            {
                unassignedExists = false;
                foreach (var field in fieldIndices)
                {
                    // unassigned; ExceptWith removes any indices already assigned
                    if (field.Value.Count > 1) {
                        unassignedExists = true;
                        field.Value.ExceptWith(assignedIndices);
                    }
                    // otherwise assign index
                    else
                    {
                        assignedIndices.Add(field.Value.First());
                        result[field.Value.First()] = field.Key;
                    }
                }
            } while (unassignedExists);

            return result;
        }
    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

    public class TicketValidator
    {
        private IDictionary<string, Rule> rules = new Dictionary<string, Rule>();
        public IList<string> Rules
        {
            get
            {
                return rules.Values.Select(r => r.Name).ToList();
            }
        }

        public void AddRule(string name)
        {
            if (!rules.ContainsKey(name))
                rules.Add(name, new Rule { Name = name });
        }

        public void AddRule(string name, int min, int max)
        {
            if (!rules.ContainsKey(name))
                rules.Add(name, new Rule { Name = name });
            rules[name].AddBound(min, max);
        }

        public void AddRule(string name, IEnumerable<Tuple<int, int>> bounds)
        {
            if (!rules.ContainsKey(name))
                rules.Add(name, new Rule { Name = name });
            foreach (var b in bounds)
            {
                rules[name].AddBound(b.Item1, b.Item2);
            }
        }

        public void AddRule(Rule rule)
        {
            if (!rules.ContainsKey(rule.Name))
                rules.Add(rule.Name, rule);
            else
                rules[rule.Name] = rule;
        }

        public void AddRules(IEnumerable<Rule> newRules)
        {
            foreach(var rule in newRules)
            {
                AddRule(rule);
            }
        }

        public bool ValidateValue(int value)
        {
            foreach (var rule in rules.Values)
            {
                if (rule.Inbounds(value))
                    return true;
            }

            return false;
        }

        public bool ValidateValues(IEnumerable<int> values)
        {
            foreach (var value in values)
            {
                if (!ValidateValue(value))
                    return false;
            }
            return true;
        }

        public IList<string> RulesBroken(int value)
        {
            IList<string> result = new List<string>();
            foreach (var rule in rules.Values)
            {
                if (!rule.Inbounds(value))
                    result.Add(rule.Name);
            }
            return result;
        }

        public class Rule
        {
            private IList<Tuple<int, int>> bounds = new List<Tuple<int, int>>();

            public string Name { get; set; }

            public void AddBound(int min, int max)
            {
                bounds.Add(new Tuple<int, int>(min, max));
            }

            public bool Inbounds(int value)
            {
                foreach(var b in bounds)
                {
                    if (b.Item1 <= value && value <= b.Item2)
                        return true;
                }
                return false;
            }
        }
    }

}