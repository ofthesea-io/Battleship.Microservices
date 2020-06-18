namespace Battleship.ScoreCard.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Infrastructure.Core.Messages;
    using Battleship.Infrastructure.Core.Models;
    using Battleship.ScoreCard.Controllers;
    using Battleship.ScoreCard.Infrastructure;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    public class ScoreCardTests
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
            string result = await this.moqScoreCardRepository.Object.GetPlayerScoreCard(this.scoreCard.SessionToken);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public async Task GetPlayerScoreCard_WhenNoAuthHeaderIsSet_ThrowsInternalServerError()
        {
            // Arrange
            this.moqScoreCardRepository.Reset();
            this.moqScoreCardRepository.Setup(q => q.GetPlayerScoreCard(this.scoreCard.SessionToken)).Returns(Task.FromResult(JsonConvert.SerializeObject(this.scoreCard)));

            // Act
            ActionResult actionResult = await this.scoreCardController.GetPlayerScoreCard();

            // Assert
            StatusCodeResult error = actionResult as StatusCodeResult;
            if (error == null)
                Assert.Fail();
            else
                Assert.AreEqual(error.StatusCode, StatusCodes.Status500InternalServerError);
        }

        [Test]
        public async Task GetPlayerScoreCard_WhenAuthHeaderIsSet_ReturnOkStatus()
        {
            // Arrange
            this.moqScoreCardRepository.Reset();
            this.moqScoreCardRepository.Setup(q => q.GetPlayerScoreCard(this.scoreCard.SessionToken)).Returns(Task.FromResult(JsonConvert.SerializeObject(this.scoreCard)));
            this.scoreCardController.ControllerContext = new ControllerContext();
            this.scoreCardController.ControllerContext.HttpContext = new DefaultHttpContext();
            this.scoreCardController.ControllerContext.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("Authorization", Guid.NewGuid().ToString()));
            
            // Act
            ActionResult actionResult = await this.scoreCardController.GetPlayerScoreCard();

            // Assert
            StatusCodeResult ok = actionResult as StatusCodeResult;
            if (ok == null)
                Assert.Fail();
            else
                Assert.AreEqual(ok.StatusCode, StatusCodes.Status200OK);
        }

        #endregion
    }
}