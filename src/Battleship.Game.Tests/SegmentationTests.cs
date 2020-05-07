namespace Battleship.Game.Tests
{
    using System;
    using System.Collections.Generic;
    using Board;
    using Enums;
    using Microservices.Infrastructure.Components;
    using Microservices.Infrastructure.Models;
    using Models;
    using NUnit.Framework;
    using Ships;
    using Utilities;

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
            var x = this.XInitialPoint;
            var y = this.GridDimension + this.Index;
            var coordinate = new Coordinate(x, y);
            var segment = new Segment(ComponentBase.Water);

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
            var x = this.XInitialPoint;
            var y = this.GridDimension + this.Index;
            var coordinate = new Coordinate(x, y);
            var segment = new Segment(ShipDirection.Vertical, new Destroyer(1));

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
            var x = this.XInitialPoint;
            var y = this.GridDimension;
            var coordinate = new Coordinate(x, y);

            // Act and Assert
            try
            {
                this.segmentation.AddSegment(coordinate, new Segment(ComponentBase.Water));
                SortedDictionary<Coordinate, Segment> range = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer())
                {
                    {coordinate, new Segment(ComponentBase.Water)}
                };
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
            var x = this.XInitialPoint;
            var y = this.GridDimension;
            var coordinate = new Coordinate(x, y);

// Act and Assert
            try
            {
                this.segmentation.AddSegment(coordinate, new Segment(ShipDirection.Horizontal, new BattleShip(1)));
                SortedDictionary<Coordinate, Segment> range = new SortedDictionary<Coordinate, Segment>
                {
                    {coordinate, new Segment(ComponentBase.Water)}
                };
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