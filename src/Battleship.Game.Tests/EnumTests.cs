namespace Battleship.Game.Tests
{
    using System;

    using Battleship.Game.Enums;
    using Battleship.Infrastructure.Core.Components;

    using NUnit.Framework;

    [TestFixture]
    public class EnumTests : ComponentBase
    {
        [Test]
        public void Boundaries_WhenCastToCorrectNumber_ReturnsTrue([Range(0, 3)] int values)
        {
            // Arrange

            // act 
            bool result = Enum.IsDefined(typeof(Boundaries), values);

            // Assert
            Assert.AreEqual(result, true);
        }

        [Test]
        public void Boundaries_WhenCastToInvalidNumber_ReturnsFalse([Range(4, 10)] int values)
        {
            // Arrange

            // act 
            bool result = Enum.IsDefined(typeof(Boundaries), values);

            // Assert
            Assert.AreEqual(result, false);
        }

        [Test]
        public void ShipDirection_WhenCastToCorrectNumber_ReturnsTrue([Range(0, 1)] int values)
        {
            // Arrange

            // act 
            bool result = Enum.IsDefined(typeof(ShipDirection), values);

            // Assert
            Assert.AreEqual(result, true);
        }

        [Test]
        public void ShipDirection_WhenCastToInvalidNumber_ReturnsFalse([Range(2, 10)] int values)
        {
            // Arrange

            // act 
            bool result = Enum.IsDefined(typeof(ShipDirection), values);

            // Assert
            Assert.AreEqual(result, false);
        }
    }
}