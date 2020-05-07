namespace Battleship.Game.Tests
{
    using System;
    using System.Collections.Generic;
    using Board;
    using Microservices.Infrastructure.Components;
    using NUnit.Framework;
    using Ships;

    [TestFixture]
    public class GridGeneratorTests : ComponentBase
    {
        private GridGenerator gridGenerator;

        private IShipRandomiser shipRandomiser;

        [Test]
        public void Board_WhenGridGenerated_ReturnOneHundredSegments()
        {
            // Arrange
            var totalSegments = this.GridDimension * this.GridDimension;
            int? result = 0;

            // Act
            try
            {
                this.gridGenerator = GridGenerator.Instance();
                result = this.gridGenerator.TotalSegments;
            }
            catch (IndexOutOfRangeException)
            {
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Fail($"{e.Message}\n{e.StackTrace}");
            }

            // Assert
            Assert.AreEqual(totalSegments, result);
        }

        [Test]
        public void Board_WhenGridGenerated_ReturnThirteenOccupiedSegments()
        {
            // Arrange
            List<IShip> ships = new List<IShip> {new BattleShip(1), new Destroyer(2), new Destroyer(3)};
            this.shipRandomiser = ShipRandomiser.Instance();
            var occupiedSegments = this.shipRandomiser.GetRandomisedShipCoordinates(ships).Count;

            // Act
            int? result = this.gridGenerator.NumberOfOccupiedSegments;

            // Assert
            Assert.AreNotEqual(occupiedSegments, result);
        }
    }
}