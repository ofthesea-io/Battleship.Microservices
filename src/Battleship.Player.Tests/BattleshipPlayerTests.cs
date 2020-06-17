namespace Battleship.Player.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Battleship.Infrastructure.Core.Messages;
    using Battleship.Player.Controllers;
    using Battleship.Player.Infrastructure;
    using Battleship.Player.Models;

    using Moq;

    using NUnit.Framework;

    public class BattleshipPlayerTests
    {
        private PlayerController playerController;

        private Mock<IPlayerRepository> moqPlayerRepository;

        private Mock<IMessagePublisher> moqMessagePublisher;

        private IEnumerable<Player> players;

        #region Methods

        [SetUp]
        public void Setup()
        {
            
            this.players = new List<Player>()
               {
                   new Player { Firstname = "John", Lastname = "Doe", Email = "johndoe@email.com" },
                   new Player { Firstname = "Jane", Lastname = "Doe", Email = "janedoe@email.com" }
               };

            this.moqPlayerRepository = new Mock<IPlayerRepository>();
            this.moqMessagePublisher = new Mock<IMessagePublisher>();

            this.playerController = new PlayerController(this.moqPlayerRepository.Object, this.moqMessagePublisher.Object);
        }

        [Test]
        public async Task GetDemoPlayer_WhenGivenNoInput()
        {
            // Arrange
            this.moqPlayerRepository.Reset();
            this.moqPlayerRepository.Setup(q => q.GetDemoPlayers()).Returns(Task.FromResult(this.players));

            // Act
            var result = await this.moqPlayerRepository.Object.GetDemoPlayers();

            // Assert
            Assert.AreEqual(this.players.ToList()[0].Firstname, result.ToList()[0].Firstname);
            Assert.AreEqual(this.players.ToList()[0].Lastname, result.ToList()[0].Lastname);
            Assert.AreEqual(this.players.ToList()[0].Email, result.ToList()[0].Email);

            Assert.AreEqual(this.players.ToList()[1].Firstname, result.ToList()[1].Firstname);
            Assert.AreEqual(this.players.ToList()[1].Lastname, result.ToList()[1].Lastname);
            Assert.AreEqual(this.players.ToList()[1].Email, result.ToList()[1].Email);
        }

        #endregion
    }
}