namespace Battleship.Game.Tests
{
    using System.Linq;
    using Controllers;
    using Infrastructure;
    using Microservices.Infrastructure.Messages;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class BoardControllerTests
    {
        [SetUp]
        public void Setup()
        {
            Mock<IGameRepository> gameRepository = new Mock<IGameRepository>();
            Mock<IMessagePublisher> messagePublisherMoq = new Mock<IMessagePublisher>();
            this.boardController = new BoardController(gameRepository.Object, messagePublisherMoq.Object);
        }

        private BoardController boardController;

        [Test]
        public void GetGamingGrid_IsValidGamingGrid_ReturnGamingGrid()
        {
            // Arrange

            // Act
            ActionResult<GamingGrid> result = this.boardController.GetGamingGrid().Result;

            // Assert
            Assert.IsInstanceOf<ActionResult<GamingGrid>>(result);
        }

        [Test]
        public void GetGamingGrid_XAxis_ReturnMoreThanOne()
        {
            // Arrange

            // Act
            ActionResult<GamingGrid> gamingGrid = this.boardController.GetGamingGrid().Result;
            var counter = gamingGrid.Value.X.Count();
            var result = counter > 0;

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public void GetGamingGrid_YAxis_ReturnMoreThanOne()
        {
            // Arrange

            // Act
            ActionResult<GamingGrid> gamingGrid = this.boardController.GetGamingGrid().Result;
            var counter = gamingGrid.Value.Y.Count();
            var result = counter > 0;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void StartGame_IfAuthorizationIsEmpty_ThrowStatus401Unauthorized()
        {
            // Arrange

            // Act
            var result = this.boardController.StartGame(1).Result as StatusCodeResult;

            // Assert
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public void StartGame_IfAuthorizationIsLoaded_StartGame()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "YWxhZGRpbjpvcGVuc2VzYW1l";
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            this.boardController.ControllerContext = controllerContext;

            // Act
            var result = this.boardController.StartGame(1).Result as StatusCodeResult;

            // Assert
            Assert.IsInstanceOf<StatusCodeResult>(result);
            Assert.That(this.boardController.Response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }
    }
}