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
            this.timestamp = DateTime.Now;
            this.auditLogs = new List<Audit>
                                 {
                                     new Audit { AuditType = AuditType.Warning, Content = "Warning message",  Timestamp = this.timestamp },
                                     new Audit { AuditType = AuditType.Error, Content = "Error message",  Timestamp = this.timestamp },
                                     new Audit { AuditType = AuditType.Info, Content = "Info content",  Timestamp = this.timestamp }
                                 };

            this.moqAuditRepository = new Mock<IAuditRepository>();
            this.moqMessagePublisher = new Mock<IMessagePublisher>();
            this.auditController = new AuditController(this.moqAuditRepository.Object, this.moqMessagePublisher.Object);
        }

        [Test]
        public async Task GetAuditLog_WhenGivenNothing_ReturnsAllLogs()
        {
            // Arrange
            this.moqAuditRepository.Reset();
            this.moqAuditRepository.Setup(q => q.GetAuditContent()).Returns(Task.FromResult(this.auditLogs));

            // Act
            IEnumerable<Audit> result = await this.moqAuditRepository.Object.GetAuditContent();

            // Assert
            Assert.AreEqual(this.auditLogs, result);
        }

        [Test]
        public async Task GetAuditLog_WhenGivenAType_ReturnAuditLogsByType()
        {
            // Arrange
            this.moqAuditRepository.Reset();
            this.moqAuditRepository.Setup(q => q.GetAuditContentByAuditType(AuditType.Warning)).Returns(Task.FromResult(this.auditLogs.Where(q => q.AuditType == AuditType.Warning)));

            // Act
            IEnumerable<Audit> result = await this.moqAuditRepository.Object.GetAuditContentByAuditType(AuditType.Warning);

            // Assert - return one or more
            Assert.GreaterOrEqual(1, result.Count());
        }

        #endregion
    }
}