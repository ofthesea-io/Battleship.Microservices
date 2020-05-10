namespace Battleship.Game.Tests
{
    using System.Collections.Generic;

    using Battleship.Microservices.Core.Components;
    using Battleship.Microservices.Core.Models;

    using Enums;

    using Models;
    using NUnit.Framework;
    using Ships;
    using Utilities;

    public class BattleshipExtensionTests : ComponentBase
    {
        #region Methods

        [Test]
        public void IsSegmentAvailable_WhenShipDoesNotInterceptHorizontally_ReturnTrue()
        {
            // Arrange
            IShip firstDestroyer = new Destroyer(1);
            IShip secondDestroyer = new Destroyer(2);

            SortedDictionary<Coordinate, Segment> segments = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer())
            {
                {new Coordinate(69, 1), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(69, 2), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(69, 3), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(69, 4), new Segment(ShipDirection.Horizontal, firstDestroyer)}
            }; // list of current segments that is not available

            KeyValuePair<Coordinate, Segment> segment = new KeyValuePair<Coordinate, Segment>(new Coordinate(69, 5), new Segment(ShipDirection.Vertical, secondDestroyer));

            // Act
            var result = segments.IsSegmentAvailable(segment.Key.X, segment.Key.Y);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsSegmentAvailable_WhenShipDoesNotInterceptVertically_ReturnTrue()
        {
            // Arrange
            IShip firstDestroyer = new Destroyer(1);
            IShip secondDestroyer = new Destroyer(2);

            SortedDictionary<Coordinate, Segment> segments = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer())
            {
                {new Coordinate(68, 3), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(69, 3), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(70, 3), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(71, 3), new Segment(ShipDirection.Horizontal, firstDestroyer)}
            }; // list of current segments that is not available

            KeyValuePair<Coordinate, Segment> segment = new KeyValuePair<Coordinate, Segment>(new Coordinate(73, 3),
                new Segment(ShipDirection.Vertical, secondDestroyer));

            // Act
            var result = segments.IsSegmentAvailable(segment.Key.X, segment.Key.Y);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsSegmentAvailable_WhenShipInterceptsHorizontally_ReturnFalse()
        {
            // Arrange
            IShip firstDestroyer = new Destroyer(1);


            SortedDictionary<Coordinate, Segment> segments = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer())
            {
                {new Coordinate(69, 1), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(69, 2), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(69, 3), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(69, 4), new Segment(ShipDirection.Horizontal, firstDestroyer)}
            }; // list of current segments that is not available 

            // Horizontal Intercepting ship
            var cooridnate = new Coordinate(69, 2);

            // Act
            var result = segments.IsSegmentAvailable(cooridnate.X, cooridnate.Y);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsSegmentAvailable_WhenShipInterceptsVertically_ReturnFalse()
        {
            // Arrange
            IShip firstDestroyer = new Destroyer(1);
            IShip secondDestroyer = new Destroyer(2);

            SortedDictionary<Coordinate, Segment> segments = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer())
            {
                {new Coordinate(69, 3), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(70, 3), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(71, 3), new Segment(ShipDirection.Horizontal, firstDestroyer)}
            }; // list of current segments that is not available

            KeyValuePair<Coordinate, Segment> segment = new KeyValuePair<Coordinate, Segment>(new Coordinate(69, 3),
                new Segment(ShipDirection.Vertical, secondDestroyer)); // fail point

            // Act
            var result = segments.IsSegmentAvailable(segment.Key.X, segment.Key.Y);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsSegmentAvailable_WhenTwoHorizontalShipsDoesNotIntercept_ReturnTrue()
        {
            // Arrange
            IShip firstDestroyer = new Destroyer(1);
            IShip secondDestroyer = new Destroyer(2);

            SortedDictionary<Coordinate, Segment> segments = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer())
            {
                {new Coordinate(68, 2), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(69, 2), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(70, 2), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(71, 2), new Segment(ShipDirection.Horizontal, firstDestroyer)}
            }; // list of current segments that is not available 

            KeyValuePair<Coordinate, Segment> segment = new KeyValuePair<Coordinate, Segment>(new Coordinate(67, 2),
                new Segment(ShipDirection.Vertical, secondDestroyer)); // pass point

            // Act
            var result = segments.IsSegmentAvailable(segment.Key.X, segment.Key.Y);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsSegmentAvailable_WhenTwoHorizontalShipsIntercept_ReturnFalse()
        {
            // Arrange
            IShip firstDestroyer = new Destroyer(1);
            IShip secondDestroyer = new Destroyer(2);

            SortedDictionary<Coordinate, Segment> segments = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer())
            {
                {new Coordinate(68, 2), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(69, 2), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(70, 2), new Segment(ShipDirection.Horizontal, firstDestroyer)},
                {new Coordinate(71, 2), new Segment(ShipDirection.Horizontal, firstDestroyer)}
            }; // list of current segments that is not available 

            // Horizontal Intercepting ship
            KeyValuePair<Coordinate, Segment> segment = new KeyValuePair<Coordinate, Segment>(new Coordinate(71, 2),
                new Segment(ShipDirection.Vertical, secondDestroyer));

            // Act
            var result = segments.IsSegmentAvailable(segment.Key.X, segment.Key.Y);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsSegmentWithInGridRange_WhenXAxisIsGreaterThanDimension_ReturnFalse()
        {
            // Arrange
            IShip firstDestroyer = new Destroyer(1);

            var x = this.XInitialPoint + this.GridDimension + this.Index;
            var y = this.Index;

            // Act
            var result = BattleshipExtensions.IsSegmentWithInGridRange(x, y);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsSegmentWithInGridRange_WhenXAxisIsLessThanXIndex_ReturnFalse()
        {
            // Arrange
            var x = this.XInitialPoint - this.Index;
            var y = this.Index;

            // Act
            var result = BattleshipExtensions.IsSegmentWithInGridRange(x, y);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsSegmentWithInGridRange_WhenXAxisIsWithinDimension_ReturnTrue()
        {
            // Arrange
            var x = this.XInitialPoint;
            var y = this.GridDimension;

            // Act
            var result = BattleshipExtensions.IsSegmentWithInGridRange(x, y);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsSegmentWithInGridRange_WhenYAxisIsGreaterThanDimension_ReturnFalse()
        {
            // Arrange
            var x = this.XInitialPoint;
            var y = this.Index + this.GridDimension;

            // Act
            var result = BattleshipExtensions.IsSegmentWithInGridRange(x, y);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsSegmentWithInGridRange_WhenYAxisIsLessThanYAxisIndex_ReturnFalse()
        {
            // Arrange
            var x = this.XInitialPoint;
            var y = 0;

            // Act
            var result = BattleshipExtensions.IsSegmentWithInGridRange(x, y);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsSegmentWithInGridRange_WhenYAxisIsWithinDimension_ReturnTrue()
        {
            // Arrange
            var x = this.XInitialPoint;
            var y = this.GridDimension;

            // Act
            var result = BattleshipExtensions.IsSegmentWithInGridRange(x, y);


// Assert
            Assert.IsTrue(result);
        }

        #endregion
    }
}