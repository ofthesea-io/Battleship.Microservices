namespace Battleship.Audit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Battleship.Audit.Controllers;
    using Battleship.Audit.Infrastructure;
    using Battleship.Microservices.Core.Messages;
    using Battleship.Microservices.Core.Models;
    using Battleship.Microservices.Core.Utilities;

    using Moq;

    using NUnit.Framework;

    public class BattleshipAuditTests
    {
        #region Fields

        private AuditController auditController;

        private IEnumerable<Audit> auditLogs;

        private Mock<IAuditRepository> moqAuditRepository;

        private Mock<IMessagePublisher> moqMessagePublisher;

        private DateTime timestamp;

        #endregion

        #region Methods

        [SetUp]
        public void Setup()
        {
            this.timestamp = DateTime.Parse("3/2/2020 12:00:00 PM");
            this.auditLogs = new List<Audit>
             {
                 new Audit { AuditTypeId = AuditType.Warning, Content = "Warning message",  Timestamp = DateTime.Parse("1/2/2020 12:00:00 AM") },
                 new Audit { AuditTypeId = AuditType.Error, Content = "Error message",  Timestamp = DateTime.Parse("1/2/2020 12:00:00 AM") },
                 new Audit { AuditTypeId = AuditType.Info, Content = "Info content",  Timestamp = DateTime.Parse("1/2/2020 12:00:00 AM") },
                 new Audit { AuditTypeId = AuditType.Warning, Content = "Warning message",  Timestamp = DateTime.Parse("1/2/2020 05:00:00 AM") },
                 new Audit { AuditTypeId = AuditType.Error, Content = "Error message",  Timestamp = DateTime.Parse("1/2/2020 05:00:00 AM") },
                 new Audit { AuditTypeId = AuditType.Info, Content = "Info content",  Timestamp = DateTime.Parse("1/2/2020 05:00:00 AM") },
                 new Audit { AuditTypeId = AuditType.Warning, Content = "Warning message",  Timestamp = DateTime.Parse("1/2/2020 10:00:00 AM") },
                 new Audit { AuditTypeId = AuditType.Error, Content = "Error message",  Timestamp = DateTime.Parse("1/2/2020 10:00:00 AM") },
                 new Audit { AuditTypeId = AuditType.Info, Content = "Info content",  Timestamp = DateTime.Parse("1/2/2020 10:00:00 AM") },
                 new Audit { AuditTypeId = AuditType.Warning, Content = "Warning message",  Timestamp = DateTime.Parse("2/2/2020 12:00:00 PM") },
                 new Audit { AuditTypeId = AuditType.Error, Content = "Error message",  Timestamp = DateTime.Parse("2/2/2020 12:00:00 PM") },
                 new Audit { AuditTypeId = AuditType.Info, Content = "Info content",  Timestamp = DateTime.Parse("2/2/2020 12:00:00 PM") },
                 new Audit { AuditTypeId = AuditType.Warning, Content = "Warning message",  Timestamp = DateTime.Parse("2/2/2020 11:00:00 PM") },
                 new Audit { AuditTypeId = AuditType.Error, Content = "Error message",  Timestamp = DateTime.Parse("3/2/2020 12:00:00 PM") },
                 new Audit { AuditTypeId = AuditType.Info, Content = "Info content",  Timestamp = DateTime.Parse("3/2/2020 12:00:00 PM") },
             };

            this.moqAuditRepository = new Mock<IAuditRepository>();
            this.moqMessagePublisher = new Mock<IMessagePublisher>();
            this.auditController = new AuditController(this.moqAuditRepository.Object, this.moqMessagePublisher.Object);
        }


        [Test]
        public async Task GetAuditLog_WhenGivenAWarningIdAnd0HoursReturnAllWarnings()
        {
            // Arrange
            var warning = AuditType.Warning;
            this.moqAuditRepository.Reset();
            this.moqAuditRepository.Setup(q => q.GetAuditContentByAuditTypeHourRange(warning, 0)).Returns(Task.FromResult(this.auditLogs.Where(q => q.AuditTypeId == warning)));
            int expectedResult = this.auditLogs.Count(q => q.AuditTypeId == warning);

            // Act
            IEnumerable<Audit> result = await this.moqAuditRepository.Object.GetAuditContentByAuditTypeHourRange(warning);

            // Assert - return one or more
            Assert.AreEqual(expectedResult, result.Count());
        }


        [Test]
        public async Task GetAuditLog_WhenGivenErrorIdAnd0Hours_ReturnAllErrors()
        {
            // Arrange
            var error = AuditType.Error;
            this.moqAuditRepository.Reset();
            this.moqAuditRepository.Setup(q => q.GetAuditContentByAuditTypeHourRange(error, 0)).Returns(Task.FromResult(this.auditLogs.Where(q => q.AuditTypeId == error)));
            int expectedResult = this.auditLogs.Count(q => q.AuditTypeId == error);

            // Act
            IEnumerable<Audit> result = await this.moqAuditRepository.Object.GetAuditContentByAuditTypeHourRange(error);

            // Assert - return one or more
            Assert.AreEqual(expectedResult, result.Count());
        }

        [Test]
        public async Task GetAuditLog_WhenGivenInfoIdAnd0Hours_ReturnAllInfo()
        {
            // Arrange
            var info = AuditType.Error;
            this.moqAuditRepository.Reset();
            this.moqAuditRepository.Setup(q => q.GetAuditContentByAuditTypeHourRange(info, 0)).Returns(Task.FromResult(this.auditLogs.Where(q => q.AuditTypeId == info)));
            int expectedResult = this.auditLogs.Count(q => q.AuditTypeId == info);

            // Act
            IEnumerable<Audit> result = await this.moqAuditRepository.Object.GetAuditContentByAuditTypeHourRange(info);

            // Assert - return one or more
            Assert.AreEqual(expectedResult, result.Count());
        }

        #endregion
    }
}