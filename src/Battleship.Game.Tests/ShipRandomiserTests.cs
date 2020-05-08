namespace Battleship.Core.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Game.Models;
    using Game.Ships;
    using Microservices.Infrastructure.Components;
    using Microservices.Infrastructure.Models;
    using NUnit.Framework;

    [TestFixture]
    public class ShipRandomiserTests : ComponentBase
    {
        private readonly IShipRandomiser shipRandomiser;

        public ShipRandomiserTests()
        {
            this.shipRandomiser = ShipRandomiser.Instance();
        }

        [Test]
        public void GetRandomisedShipCoordinates_GetNumberOfBattleships_ReturnOne()
        {
            // Arrange
            var numberOfBattleships = 1;
            List<IShip> ships = new List<IShip> {new BattleShip(1), new Destroyer(2), new Destroyer(3)};

            // Act
            SortedDictionary<Coordinate, Segment> segments = this.shipRandomiser.GetRandomisedShipCoordinates(ships);

            // Make sure that the HashCodes are different
            IEnumerable<IShip> battleship = segments.Where(s => s.Value.Ship.ShipChar == ComponentBase.BattleShipCode).Select(s => s.Value.Ship);
            var counter = battleship.GroupBy(q => q.GetHashCode()).Count();

            // Assert
            Assert.AreEqual(counter, numberOfBattleships);
        }

        [Test]
        public void GetRandomisedShipCoordinates_GetNumberOfDestroyers_ReturnTwo()
        {
            // Arrange
            var numberOfDestroyers = 2;
            List<IShip> ships = new List<IShip> {new BattleShip(1), new Destroyer(2), new Destroyer(3)};

            // Act

            // Make sure we only get one set of hash codes
            SortedDictionary<Coordinate, Segment> segments = this.shipRandomiser.GetRandomisedShipCoordinates(ships);
            IEnumerable<IShip> battleship = segments.Where(s => s.Value.Ship.ShipChar == ComponentBase.DestroyerCode).Select(s => s.Value.Ship);

            var counter = battleship.GroupBy(q => q.GetHashCode()).Count();


// Assert
            Assert.AreEqual(counter, numberOfDestroyers);
        }

        [Test]
        public void GetRandomisedShipCoordinates_OneBattleShip_ReturnFiveSegments()
        {
            // Arrange
            var numberOfSegments = 5;
            List<IShip> ships = new List<IShip> {new BattleShip(1), new Destroyer(2), new Destroyer(3)};

            // Act
            SortedDictionary<Coordinate, Segment> segments = this.shipRandomiser.GetRandomisedShipCoordinates(ships);
            var counter = segments.Count(q => q.Value.Ship.ShipChar == ComponentBase.BattleShipCode);

            // Assert
            Assert.AreEqual(counter, numberOfSegments);
        }

        [Test]
        public void GetRandomisedShipCoordinates_TwoDestroyers_ReturnEightSegments()
        {
            // Arrange
            var numberOfSegments = 8;
            List<IShip> ships = new List<IShip> {new BattleShip(1), new Destroyer(2), new Destroyer(3)};

            // Act
            SortedDictionary<Coordinate, Segment> segments = this.shipRandomiser.GetRandomisedShipCoordinates(ships);
            var counter = segments.Count(q => q.Value.Ship.ShipChar == ComponentBase.DestroyerCode);

            // Assert
            Assert.AreEqual(counter, numberOfSegments);
        }

        [Test]
        public void GetRandomisedShipCoordinates_WhenShipsRanomised_ReturnThirteenSegments()
        {
            // Arrange
            // should always return 13 segments
            // 1x Battleship (5 squares)
            // 2x Destroyers(4 squares)
            // 5 + 4 + 4
            var segmentCounter = 13;
            List<IShip> ships = new List<IShip> {new BattleShip(1), new Destroyer(2), new Destroyer(3)};

            // Act
            SortedDictionary<Coordinate, Segment> segments = this.shipRandomiser.GetRandomisedShipCoordinates(ships);

            // Assert
            Assert.AreEqual(segmentCounter, segments.Count);
        }
    }
}