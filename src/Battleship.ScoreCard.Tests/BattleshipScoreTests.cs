namespace Battleship.ScoreCard.Tests
{
    using System.Threading.Tasks;

    using Battleship.Infrastructure.Core.Messages;
    using Battleship.Infrastructure.Core.Models;
    using Battleship.ScoreCard.Controllers;
    using Battleship.ScoreCard.Infrastructure;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    public class BattleshipScoreTests
    {
        #region Fields

        private Mock<IMessagePublisher> moqMessagePublisher;

        private Mock<IScoreCardRepository> moqScoreCardRepository;

        private ScoreCard scoreCard;

        private ScoreCardController scoreCardController;

        #endregion

        #region Methods

        [SetUp]
        public void Setup()
        {
            this.scoreCard = new ScoreCard
                                 {
                                     IsCompleted = false,
                                     IsHit = true,
                                     Hit = 5,
                                     Miss = 25,
                                     SessionToken = "SessionToken",
                                     Sunk = 1
                                 };

            this.moqMessagePublisher = new Mock<IMessagePublisher>();
            this.moqScoreCardRepository = new Mock<IScoreCardRepository>();
            this.scoreCardController = new ScoreCardController(this.moqScoreCardRepository.Object, this.moqMessagePublisher.Object);
        }

        [Test]
        public async Task GetPlayerScoreCard_ReturnsPlayerScoreCardAsSerialized()
        {
            // Arrange
            this.moqScoreCardRepository.Reset();
            this.moqScoreCardRepository.Setup(q => q.GetPlayerScoreCard(this.scoreCard.SessionToken)).Returns(Task.FromResult(JsonConvert.SerializeObject(this.scoreCard)));
            string expectedResult = JsonConvert.SerializeObject(this.scoreCard);

            // Act
            var result = await this.moqScoreCardRepository.Object.GetPlayerScoreCard(this.scoreCard.SessionToken);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion
    }
}