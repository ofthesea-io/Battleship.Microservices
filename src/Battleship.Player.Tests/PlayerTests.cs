namespace Battleship.Player.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Battleship.Infrastructure.Core.Messages;
    using Battleship.Player.Controllers;
    using Battleship.Player.Infrastructure;
    using Battleship.Player.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    public class PlayerTests
    {
        #region Fields

        private readonly Guid sessionId = Guid.NewGuid();

        private Authenticated authenticated;

        private Mock<IMessagePublisher> moqMessagePublisher;

        private Mock<IPlayerRepository> moqPlayerRepository;

        private Player player;

        private PlayerController playerController;

        private IEnumerable<Player> players;

        #endregion

        #region Methods

        [SetUp]
        public void Setup()
        {
            this.player = new Player
              {
                  Firstname = "Test",
                  Lastname = "Test",
                  Email = "test@email.com",
                  Password = "testing",
                  PlayerId = Guid.NewGuid()
              };

            this.players = new List<Player>
               {
                   new Player
                       {
                           Firstname = "John",
                           Lastname = "Doe",
                           Email = "johndoe@email.com"
                       },
                   new Player
                       {
                           Firstname = "Jane",
                           Lastname = "Doe",
                           Email = "janedoe@email.com"
                       }
               };

            this.authenticated = new Authenticated
             {
                 IsAdmin = 0,
                 IsDemo = 0,
                 Level = 1
             };

            this.moqPlayerRepository = new Mock<IPlayerRepository>();
            this.moqMessagePublisher = new Mock<IMessagePublisher>();

            this.playerController = new PlayerController(this.moqPlayerRepository.Object, this.moqMessagePublisher.Object);
        }

        [Test]
        public async Task GetDemoPlayerRepository_WhenGivenNoInput_ReturnsDemoPlayersSerialized()
        {
            // Arrange
            this.moqPlayerRepository.Reset();
            this.moqPlayerRepository.Setup(q => q.GetDemoPlayers()).ReturnsAsync(this.players);

            // Act
            IEnumerable<Player> result = await this.moqPlayerRepository.Object.GetDemoPlayers();

            // Assert
            IEnumerable<Player> resultArray = result as Player[] ?? result.ToArray();
            Assert.AreEqual(this.players.ToList()[0].Firstname, resultArray.ToList()[0].Firstname);
            Assert.AreEqual(this.players.ToList()[0].Lastname, resultArray.ToList()[0].Lastname);
            Assert.AreEqual(this.players.ToList()[0].Email, resultArray.ToList()[0].Email);

            Assert.AreEqual(this.players.ToList()[1].Firstname, resultArray.ToList()[1].Firstname);
            Assert.AreEqual(this.players.ToList()[1].Lastname, resultArray.ToList()[1].Lastname);
            Assert.AreEqual(this.players.ToList()[1].Email, resultArray.ToList()[1].Email);
        }

        [Test]
        public async Task GetDemoPlayerController_WhenGivenNoInput_ReturnsActionResult()
        {
            // Arrange
            this.moqPlayerRepository.Reset();
            this.moqPlayerRepository.Setup(q => q.GetDemoPlayers()).ReturnsAsync(this.players);
            string expectedResult = JsonConvert.SerializeObject(this.players);

            // Act
            ActionResult actionResult = await this.playerController.GetDemoPlayers();

            // Assert
            OkObjectResult ok = (OkObjectResult)actionResult;
            Assert.IsInstanceOf<ActionResult>(actionResult);
            if (ok != null)
                Assert.AreEqual(ok.Value, expectedResult);
            else
                Assert.Fail();
        }

        [Test]
        public async Task IsAuthenticated_GivenSessionId_ReturnValidSession()
        {
            // Arrange
            string sessionId = Guid.NewGuid().ToString();
            this.moqPlayerRepository.Reset();
            this.moqPlayerRepository.Setup(q => q.IsAuthenticated(sessionId)).ReturnsAsync(this.authenticated);
            string expectedResult = JsonConvert.SerializeObject(this.authenticated);

            // Act 
            ActionResult result = await this.playerController.IsAuthenticated(sessionId);
            OkObjectResult ok = (OkObjectResult)result;

            // Assert
            if (ok != null)
                Assert.AreEqual(ok.Value, expectedResult);
            else
                Assert.Fail();
        }

        [Test]
        public async Task CreatePlayer_WhenMissingFirstname_ReturnNoContentStatusCode()
        {
            // Arrange
            this.moqPlayerRepository.Reset();
            this.moqPlayerRepository.Setup(q => q.CreatePlayer(this.player)).ReturnsAsync(this.sessionId);
            this.player.Firstname = string.Empty;

            // Act
            ActionResult result = await this.playerController.CreatePlayer(this.player);
            StatusCodeResult statusCodeResult = (StatusCodeResult)result;

            // Assert
            if (statusCodeResult != null)
                Assert.AreEqual(statusCodeResult.StatusCode, StatusCodes.Status204NoContent);
            else
                Assert.Fail();
        }

        [Test]
        public async Task CreatePlayer_WhenMissingLastname_ReturnNoContentStatusCode()
        {
            // Arrange
            this.moqPlayerRepository.Reset();
            this.moqPlayerRepository.Setup(q => q.CreatePlayer(this.player)).ReturnsAsync(this.sessionId);
            this.player.Lastname = string.Empty;

            // Act
            ActionResult result = await this.playerController.CreatePlayer(this.player);
            StatusCodeResult statusCodeResult = (StatusCodeResult)result;

            // Assert
            if (statusCodeResult != null)
                Assert.AreEqual(statusCodeResult.StatusCode, StatusCodes.Status204NoContent);
            else
                Assert.Fail();
        }

        [Test]
        public async Task CreatePlayer_WhenMissingEmail_ReturnNoContentStatusCode()
        {
            // Arrange
            this.moqPlayerRepository.Reset();
            this.moqPlayerRepository.Setup(q => q.CreatePlayer(this.player)).ReturnsAsync(this.sessionId);
            this.player.Email = string.Empty;

            // Act
            ActionResult result = await this.playerController.CreatePlayer(this.player);
            StatusCodeResult statusCodeResult = (StatusCodeResult)result;

            // Assert
            if (statusCodeResult != null)
                Assert.AreEqual(statusCodeResult.StatusCode, StatusCodes.Status204NoContent);
            else
                Assert.Fail();
        }

        [Test]
        public async Task CreatePlayer_WhenMissingPassword_ReturnNoContentStatusCode()
        {
            // Arrange
            this.moqPlayerRepository.Reset();
            this.moqPlayerRepository.Setup(q => q.CreatePlayer(this.player)).ReturnsAsync(this.sessionId);
            this.player.Password = string.Empty;

            // Act
            ActionResult result = await this.playerController.CreatePlayer(this.player);
            StatusCodeResult statusCodeResult = (StatusCodeResult)result;

            // Assert
            if (statusCodeResult != null)
                Assert.AreEqual(statusCodeResult.StatusCode, StatusCodes.Status204NoContent);
            else
                Assert.Fail();
        }

        [Test]
        public async Task DemoLogin_WhenNoPlayerFound_ReturnBadRequest()
        {
            // Arrange
            this.moqPlayerRepository.Reset();
            this.moqPlayerRepository.Setup(q => q.DemoLogin(Guid.NewGuid())).ReturnsAsync(new Player());

            // Act
            ActionResult result = await this.playerController.DemoLogin(Guid.NewGuid());
            StatusCodeResult statusCodeResult = (StatusCodeResult)result;

            // Assert
            if (statusCodeResult != null)
                Assert.AreEqual(statusCodeResult.StatusCode, StatusCodes.Status400BadRequest);
            else
                Assert.Fail();
        }

        [Test]
        public async Task DemoLogin_WhenGivenPlayerId_ReturnsOkStatusCode()
        {
            // Arrange
            this.moqPlayerRepository.Reset();
            this.moqPlayerRepository.Setup(q => q.DemoLogin(this.player.PlayerId)).ReturnsAsync(this.player);

            // Act
            ActionResult result = await this.playerController.DemoLogin(this.player.PlayerId);
            OkObjectResult ok = (OkObjectResult)result;

            // Assert
            if (ok != null)
                Assert.AreEqual(ok.StatusCode, StatusCodes.Status200OK);
            else
                Assert.Fail();
        }

        #endregion
    }
}