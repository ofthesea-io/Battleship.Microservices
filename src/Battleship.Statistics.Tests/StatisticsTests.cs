namespace Battleship.Statistics.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Infrastructure.Core.Messages;
    using Battleship.Infrastructure.Core.Models;
    using Battleship.Statistics.Controllers;
    using Battleship.Statistics.Infrastructure;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    public class StatisticsTests
    {
        #region Fields

        private Mock<IMessagePublisher> moqMessagePublisher;

        private Mock<IStatisticsRepository> moqStatisticsRepository;

        private StatisticController statisticController;

        private IEnumerable<Statistics> statistics;

        #endregion

        #region Methods

        [SetUp]
        public void Setup()
        {
            this.statistics = new List<Statistics>
              {
                  new Statistics
                      {
                          CompletedGames = 1,
                          CompletedOn = DateTime.Today,
                          Email = "jakes@email.com",
                          FullName = "Jakes Potgieter",
                          WinningPercentage = 34.2
                      },
                  new Statistics
                      {
                          CompletedGames = 4,
                          CompletedOn = DateTime.Today,
                          Email = "karen@email.com",
                          FullName = "Karen Jones",
                          WinningPercentage = 87.2
                      }
              };

            this.moqStatisticsRepository = new Mock<IStatisticsRepository>();
            this.moqMessagePublisher = new Mock<IMessagePublisher>();
            this.statisticController = new StatisticController(this.moqMessagePublisher.Object, this.moqStatisticsRepository.Object);
        }

        [Test]
        public async Task GetTopTenPlayerController_WhenGivenNoInput_ReturnsActionResult()
        {
            // Arrange
            this.moqStatisticsRepository.Reset();
            this.moqStatisticsRepository.Setup(q => q.GetTopTenPlayers()).ReturnsAsync(this.statistics);
            
            // Act
            var actionResult = await this.statisticController.GetTopTenPlayers();

            // Assert
            Assert.IsInstanceOf<ActionResult>(actionResult);
        }

        [Test]
        public async Task GetTopTenPlayers_WhenGivenNoInput_ReturnsList()
        {
            // Arrange
            this.moqStatisticsRepository.Reset();
            this.moqStatisticsRepository.Setup(q => q.GetTopTenPlayers()).ReturnsAsync(this.statistics);

            // Act
            var actionResult = await this.statisticController.GetTopTenPlayers();
            var ok = actionResult as OkObjectResult;

            // Assert
            if (ok != null)
                Assert.AreEqual(ok.StatusCode, StatusCodes.Status200OK);
            else
                Assert.Fail();

        }

        #endregion
    }
}