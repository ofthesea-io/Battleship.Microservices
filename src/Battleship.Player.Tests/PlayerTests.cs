namespace Battleship.Player.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Battleship.Infrastructure.Core.Messages;
    using Battleship.Player.Controllers;
    using Battleship.Player.Infrastructure;
    using Battleship.Player.Models;

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    public class PlayerTests
    {
        private PlayerController playerController;

        private Mock<IPlayerRepository> moqPlayerRepository;

        private Mock<IMessagePublisher> moqMessagePublisher;

        private IEnumerable<Player> players;

        private Authenticated authenticated;

        #region Methods

        [SetUp]
        public void Setup()
        {

            this.players = new List<Player>()
               {
                   new Player { Firstname = "John", Lastname = "Doe", Email = "johndoe@email.com" },
                   new Player { Firstname = "Jane", Lastname = "Doe", Email = "janedoe@email.com" }
               };

            this.authenticated = new Authenticated()
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
            var result = await this.moqPlayerRepository.Object.GetDemoPlayers();

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
            var actionResult = await this.playerController.GetDemoPlayers();

            // Assert
            var ok = actionResult as OkObjectResult;
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
            var result = await this.playerController.IsAuthenticated(sessionId);
            var ok = result as OkObjectResult;

            // Assert
            if (ok != null) 
                Assert.AreEqual(ok.Value, expectedResult);
            else
                Assert.Fail();
        }

        #endregion
    }
}