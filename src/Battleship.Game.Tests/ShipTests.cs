namespace Battleship.Game.Tests
{
    using Microservices.Infrastructure.Components;
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
            for (int i = 0; i <= battleShip.ShipLength; i++)
            {
                battleShip.ShipHit++;
            }
            
            bool isSunk = battleShip.IsShipSunk;

            // Assert
            Assert.IsTrue(isSunk);
        }

        [Test]
        public void Destroyer_HitCounterEqualsShipLength_ReturnIsShipSunkTrue()
        {
            // Arrange
            IShip destroyer = new Destroyer(1);

            // Act
            for (int i = 0; i <= destroyer.ShipLength; i++)
            {
                destroyer.ShipHit++;
            }

            bool isSunk = destroyer.IsShipSunk;

            // Assert
            Assert.IsTrue(isSunk);
        }
    }
}