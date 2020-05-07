namespace Battleship.Game.Board
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microservices.Infrastructure.Components;
    using Microservices.Infrastructure.Models;
    using Models;
    using Utilities;

    public class Segmentation : ComponentBase, ISegmentation
    {
        #region Fields

        private static volatile Segmentation instance;

        private readonly SortedDictionary<Coordinate, Segment> segmentation;

        #endregion

        #region Constructors

        protected Segmentation()
        {
            this.segmentation = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer());
        }

        #endregion

        #region Methods

        public static Segmentation Instance()
        {
            if (Segmentation.instance == null)
                lock (ComponentBase.SyncObject)
                    if (Segmentation.instance == null)
                        Segmentation.instance = new Segmentation();

            return Segmentation.instance;
        }

        #endregion

        #region ISegmentation Members

        public void AddSegment(Coordinate coordinate, Segment segment)
        {
            if (!BattleshipExtensions.IsSegmentWithInGridRange(coordinate.X, coordinate.Y))
                throw new IndexOutOfRangeException();

            this.segmentation.Add(coordinate, new Segment(segment.Character));
        }

        public void UpdateSegment(Coordinate coordinate, Segment segment)
        {
            if (!BattleshipExtensions.IsSegmentWithInGridRange(coordinate.X, coordinate.Y))
                throw new IndexOutOfRangeException();

            var item = this.segmentation.FirstOrDefault(q => q.Key.X == coordinate.X && q.Key.Y == coordinate.Y).Value;

            if (item != null)
            {
                item.IsEmpty = false;
                item.Character = segment.Character;

                if (segment.Ship != null)
                {
                    item.Ship = segment.Ship;
                    item.ShipDirection = segment.ShipDirection;
                }
            }
        }

        public void UpdateSegmentRange(SortedDictionary<Coordinate, Segment> segments)
        {
            foreach (KeyValuePair<Coordinate, Segment> segment in segments)
            {
                if (!BattleshipExtensions.IsSegmentWithInGridRange(segment.Key.X, segment.Key.Y))
                    throw new IndexOutOfRangeException();

                var item = this.segmentation.FirstOrDefault(q => q.Key.X == segment.Key.X && q.Key.Y == segment.Key.Y)
                               .Value;

                if (item != null)
                {
                    if (item.IsEmpty & segment.Value.IsEmpty) throw new ArgumentException();

                    item.IsEmpty = false;
                    if (segment.Value.Ship != null)
                    {
                        item.Ship = segment.Value.Ship;
                        item.ShipDirection = segment.Value.ShipDirection;
                    }
                }
            }
        }

        public SortedDictionary<Coordinate, Segment> GetSegments()
        {
            return this.segmentation;
        }

        public Segment GetSegment(int x, int y)
        {
            return this.segmentation.FirstOrDefault(q => q.Key.X == x && q.Key.Y == y).Value;
        }

        #endregion ISegmentation Members
    }
}