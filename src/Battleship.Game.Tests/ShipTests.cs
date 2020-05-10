namespace Battleship.Game.Tests
{
    using Battleship.Microservices.Core.Components;

    using NUnit.Framework;
    using Ships;

    [TestFixture]
    public class ShipTests : ComponentBase
    {
        [Test]
        public void BattleShip_HitCounterEqualsShipLength_ReturnIsShipSunkTrue()
        {
            // Arrange
            IShip battleShip = new BattleShip(1);

            // Act
            for (var i = 0; i <= battleShip.ShipLength; i++) battleShip.ShipHit++;

            var isSunk = battleShip.IsShipSunk;

            // Assert
            Assert.IsTrue(isSunk);
        }

        [Test]
        public void Destroyer_HitCounterEqualsShipLength_ReturnIsShipSunkTrue()
        {
            // Arrange
            IShip destroyer = new Destroyer(1);

            // Act
            for (var i = 0; i <= destroyer.ShipLength; i++) destroyer.ShipHit++;

            var isSunk = destroyer.IsShipSunk;

            // Assert
            Assert.IsTrue(isSunk);
        }
    }
}