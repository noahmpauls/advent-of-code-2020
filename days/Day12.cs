using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace days
{
    public class Day12
    {
        //######################################################################
        // Part 1 + 2 Solutions
        //######################################################################

        public static int Part1()
        {
            const string path = Helpers.inputPath + @"\day12\input.txt";
            IList<Tuple<char, int>> inputs = ProcessInputFile(path);


            Ship ship = new Ship();
            foreach(var a in inputs)
            {
                char action = a.Item1;
                int value = a.Item2;
                switch (action)
                {
                    case 'N':
                        ship.ShipNorth(value);
                        break;
                    case 'E':
                        ship.ShipEast(value);
                        break;
                    case 'S':
                        ship.ShipSouth(value);
                        break;
                    case 'W':
                        ship.ShipWest(value);
                        break;
                    case 'F':
                        ship.ShipForward(value);
                        break;
                    case 'L':
                        ship.ShipTurnLeft(value / 90);
                        break;
                    case 'R':
                        ship.ShipTurnRight(value / 90);
                        break;
                }
            }

            var position = ship.Position;
            int distance = Math.Abs(position.Item1) + Math.Abs(position.Item2);
            return distance;
        }

        public static int Part2()
        {
            const string path = Helpers.inputPath + @"\day12\input.txt";
            IList<Tuple<char, int>> inputs = ProcessInputFile(path);


            Ship ship = new Ship();
            foreach (var a in inputs)
            {
                char action = a.Item1;
                int value = a.Item2;
                switch (action)
                {
                    case 'N':
                        ship.WaypointNorth(value);
                        break;
                    case 'E':
                        ship.WaypointEast(value);
                        break;
                    case 'S':
                        ship.WaypointSouth(value);
                        break;
                    case 'W':
                        ship.WaypointWest(value);
                        break;
                    case 'F':
                        ship.ShipToWaypoint(value);
                        break;
                    case 'L':
                        ship.WaypointRotateLeft(value / 90);
                        break;
                    case 'R':
                        ship.WaypointRotateRight(value / 90);
                        break;
                }
            }

            var position = ship.Position;
            int distance = Math.Abs(position.Item1) + Math.Abs(position.Item2);
            return distance;
        }

        //######################################################################
        // Methods
        //######################################################################

        public static IList<Tuple<char, int>> ProcessInputFile(string path)
        {
            return Helpers.ProcessInputFile(path, ExtractAction);
        }

        public static Tuple<char, int> ExtractAction(string line)
        {
            Regex rx = new Regex("(?<action>N|S|E|W|F|L|R)(?<value>[0-9]+)");
            Match match = rx.Match(line);
            char action = match.Groups["action"].Value[0];
            int value = int.Parse(match.Groups["value"].Value);
            return new Tuple<char, int>(action, value);
        }

    }

    //##########################################################################
    // Helper Classes
    //##########################################################################

    // public class Day/* day */Result : DayResult<void, void> { }

    public class Ship
    {
        public Tuple<int, int> Position { get; set; }
        public Tuple<int, int> Direction { get; set; }
        public Tuple<int, int> WaypointOffset { get; set; }

        public Ship()
        {
            Position = new Tuple<int, int>(0, 0);
            Direction = new Tuple<int, int>(1, 0);
            WaypointOffset = new Tuple<int, int>(10, 1);
        }

        //######################################################################
        // Ship Movement
        //######################################################################

        public Tuple<int, int> ShipNorth(int amount)
        {
            Position = new Tuple<int, int>(Position.Item1, Position.Item2 + amount);
            return Position;
        }

        public Tuple<int, int> ShipEast(int amount)
        {
            Position = new Tuple<int, int>(Position.Item1 + amount, Position.Item2);
            return Position;
        }

        public Tuple<int, int> ShipSouth(int amount)
        {
            Position = new Tuple<int, int>(Position.Item1, Position.Item2 - amount);
            return Position;
        }

        public Tuple<int, int> ShipWest(int amount)
        {
            Position = new Tuple<int, int>(Position.Item1 - amount, Position.Item2);
            return Position;
        }

        public Tuple<int, int> ShipForward(int amount)
        {
            Tuple<int, int> movement = new Tuple<int, int>(
                Direction.Item1 * amount, 
                Direction.Item2 * amount
            );
            Position = new Tuple<int, int>(
                Position.Item1 + movement.Item1,
                Position.Item2 + movement.Item2
            );
            return Position;
        }

        public Tuple<int, int> ShipToWaypoint()
        {
            Position = new Tuple<int, int>(
                Position.Item1 + WaypointOffset.Item1,
                Position.Item2 + WaypointOffset.Item2
            );
            return Position;
        }

        public Tuple<int, int> ShipToWaypoint(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ShipToWaypoint();
            }
            return Position;
        }

        public Tuple<int, int> ShipTurnRight(int turnCount)
        {
            int turns = turnCount % 4;
            for (int t = 0; t < turns; t++)
            {
                Direction = OneShipTurnRight();
            }
            return Direction;
        }

        private Tuple<int, int> OneShipTurnRight()
        {
            return new Tuple<int, int>(
                Direction.Item2,
                -Direction.Item1
            );
        }

        public Tuple<int, int> ShipTurnLeft(int turnCount)
        {
            int turns = turnCount % 4;
            for (int t = 0; t < turns; t++)
            {
                Direction = OneShipTurnLeft();
            }
            return Direction;
        }

        private Tuple<int, int> OneShipTurnLeft()
        {
            return new Tuple<int, int>(
                -Direction.Item2,
                Direction.Item1
            );
        }

        //######################################################################
        // Waypoint Movement
        //######################################################################

        public Tuple<int, int> WaypointNorth(int amount)
        {
            WaypointOffset = new Tuple<int, int>(WaypointOffset.Item1, WaypointOffset.Item2 + amount);
            return WaypointOffset;
        }

        public Tuple<int, int> WaypointEast(int amount)
        {
            WaypointOffset = new Tuple<int, int>(WaypointOffset.Item1 + amount, WaypointOffset.Item2);
            return WaypointOffset;
        }

        public Tuple<int, int> WaypointSouth(int amount)
        {
            WaypointOffset = new Tuple<int, int>(WaypointOffset.Item1, WaypointOffset.Item2 - amount);
            return WaypointOffset;
        }

        public Tuple<int, int> WaypointWest(int amount)
        {
            WaypointOffset = new Tuple<int, int>(WaypointOffset.Item1 - amount, WaypointOffset.Item2);
            return WaypointOffset;
        }

        public Tuple<int, int> WaypointRotateRight(int turnCount)
        {
            int turns = turnCount % 4;
            for (int t = 0; t < turns; t++)
            {
                WaypointOffset = OneWaypointRotateRight();
            }
            return WaypointOffset;
        }

        private Tuple<int, int> OneWaypointRotateRight()
        {
            return new Tuple<int, int>(
                WaypointOffset.Item2,
                -WaypointOffset.Item1
            );
        }

        public Tuple<int, int> WaypointRotateLeft(int turnCount)
        {
            int turns = turnCount % 4;
            for (int t = 0; t < turns; t++)
            {
                WaypointOffset = OneWaypointRotateLeft();
            }
            return WaypointOffset;
        }

        private Tuple<int, int> OneWaypointRotateLeft()
        {
            return new Tuple<int, int>(
                -WaypointOffset.Item2,
                WaypointOffset.Item1
            );
        }
    }
}

