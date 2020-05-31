namespace Battleship.Game.Tests
{
    using System;
    using System.Collections.Generic;

    using Battleship.Game.Board;
    using Battleship.Game.Enums;
    using Battleship.Game.Models;
    using Battleship.Game.Ships;
    using Battleship.Game.Utilities;
    using Battleship.Infrastructure.Core.Components;
    using Battleship.Infrastructure.Core.Models;

    using NUnit.Framework;

    [TestFixture]
    public class SegmentationTests : ComponentBase
    {
        private readonly IShipRandomiser shipRandomiser;

        private readonly ISegmentation segmentation;

        public SegmentationTests()
        {
            this.shipRandomiser = ShipRandomiser.Instance();
            this.segmentation = Segmentation.Instance();
        }

        [Test]
        public void Add_AddSegmentYAxisNotInGridDimension_ThrowIndexOutOfRangeException()
        {
            // Arrange 
            int x = this.XInitialPoint;
            int y = this.GridDimension + this.Index;
            Coordinate coordinate = new Coordinate(x, y);
            Segment segment = new Segment();

            // Act and Assert
            try
            {
                this.segmentation.AddSegment(coordinate, segment);
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void UpdateSegment_SegmentOutOfRange_ThrowIndexOutOfRangeException()
        {
            // Arrange 
            int x = this.XInitialPoint;
            int y = this.GridDimension + this.Index;
            Coordinate coordinate = new Coordinate(x, y);
            Segment segment = new Segment(ShipDirection.Vertical, new Destroyer(1));

            // Act and Assert
            try
            {
                this.segmentation.UpdateSegment(coordinate, segment);
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void UpdateSegmentRange_CantUpdateAEmptySegmentWithAnotherEmptySegment_TrowArgumentException()
        {
            // Arrange 
            int x = this.XInitialPoint;
            int y = this.GridDimension;
            Coordinate coordinate = new Coordinate(x, y);

            // Act and Assert
            try
            {
                this.segmentation.AddSegment(coordinate, new Segment());
                SortedDictionary<Coordinate, Segment> range = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer()) { { coordinate, new Segment() } };
                this.segmentation.UpdateSegmentRange(range);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void UpdateSegmentRange_CantUpdateFilledSegmentWithEmptySegment_TrowArgumentException()
        {
            // Arrange 
            int x = this.XInitialPoint;
            int y = this.GridDimension;
            Coordinate coordinate = new Coordinate(x, y);

            // Act and Assert
            try
            {
                this.segmentation.AddSegment(coordinate, new Segment(ShipDirection.Horizontal, new BattleShip(1)));
                SortedDictionary<Coordinate, Segment> range = new SortedDictionary<Coordinate, Segment> { { coordinate, new Segment() } };
                this.segmentation.UpdateSegmentRange(range);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
        }

        // This test does NOT fail in debug mode. The test runner has an issue which has been logged to the NUnit Team
        // [Test]
        // public void UpdateSegmentRange_CanUpdateSegmentWithFilledSegment_ReturnsVoidOrThrowsException()
        // {
        // // Arrange 
        // int x = this.XInitialPoint;
        // int y = this.GridDimension;
        // Coordinate coordinate = new Coordinate(x, y);

        // // Act and Assert
        // try
        // {
        // this.segmentation.AddSegment(coordinate, new Segment(Water));
        // SortedDictionary<Coordinate, Segment> range = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer())
        // {
        // { coordinate, new Segment(ShipDirection.Horizontal, new BattleShip(1)) }
        // };
        // this.segmentation.UpdateSegmentRange(range);
        // Assert.IsTrue(true);
        // }
        // catch (ArgumentException)
        // {
        // Assert.Fail();
        // }
        // }
    }
}