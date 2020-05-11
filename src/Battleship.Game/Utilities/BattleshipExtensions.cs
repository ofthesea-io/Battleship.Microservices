namespace Battleship.Game.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Battleship.Game.Models;
    using Battleship.Game.Ships;
    using Battleship.Microservices.Core.Models;

    public static class BattleshipExtensions
    {
        #region Fields

        private static readonly int Index = 1;

        private static readonly int XInitialPoint = 65;

        private static readonly int GridDimension = 12;

        #endregion

        #region Methods

        public static bool IsSegmentAvailable<TSource>(this IEnumerable<TSource> source, int x, int y)
        {
            bool result = true;

            if (source.Any())
            {
                SortedDictionary<Coordinate, Segment> segments = (SortedDictionary<Coordinate, Segment>)source;
                bool isTaken = segments.Any(q => q.Key.X == x && q.Key.Y == y && !q.Value.IsEmpty);
                if (isTaken) result = false;
            }

            return result;
        }

        public static bool AddRange<TSource>(this IEnumerable<TSource> source, SortedDictionary<Coordinate, Segment> range)
        {
            bool result = true;

            if (range.Any())
            {
                SortedDictionary<Coordinate, Segment> segments = (SortedDictionary<Coordinate, Segment>)source;

                if (segments != null)
                    foreach (KeyValuePair<Coordinate, Segment> pair in range)
                        segments.Add(pair.Key, pair.Value);
            }

            return result;
        }

        public static bool IsSegmentWithInGridRange(int? x, int? y)
        {
            bool result = false;

            if (x == null || y == null) return false;

            int maxXLength = BattleshipExtensions.XInitialPoint + BattleshipExtensions.GridDimension - BattleshipExtensions.Index;

            // Test the X and Y Axis coordinates
            if (x >= BattleshipExtensions.XInitialPoint && x <= maxXLength && y >= BattleshipExtensions.Index && y <= BattleshipExtensions.GridDimension) result = true;

            return result;
        }

        public static List<IShip> GetRandomShips(int numberOfShips)
        {
            List<IShip> ships = new List<IShip>(numberOfShips);

            for (int i = 1; i <= numberOfShips; i++)
            {
                if (i % 2 == 0)
                    ships.Add(new Destroyer(i));
                else
                    ships.Add(new BattleShip(i));
            }

            return ships;
        }

        public static string AuthorisationToken(string bearer)
        {
            if (string.IsNullOrEmpty(bearer)) throw new NullReferenceException();

            return bearer.Split(' ')[1];
        }

        #endregion
    }
}