namespace Battleship.Infrastructure.Core.Tests
{
    using System;

    using Battleship.Infrastructure.Core.Utilities;

    using NUnit.Framework;

    public class EnumTests
    {
        #region Methods

        [Test]
        public void AuditType_WhenCastToCorrectNumber_ReturnsTrue([Range(1, 3)] int values)
        {
            // Arrange

            // act 
            bool result = Enum.IsDefined(typeof(AuditType), values);

            // Assert
            Assert.AreEqual(result, true);
        }


        [Test]
        public void AuditType_WhenCastToInvalidNumber_ReturnsFalse([Range(4, 10)] int values)
        {
            // Arrange

            // act 
            bool result = Enum.IsDefined(typeof(AuditType), values);

            // Assert
            Assert.AreEqual(result, false);
        }
        #endregion
    }
}