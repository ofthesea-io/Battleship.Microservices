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
    using Battleship.Microservices.Infrastructure.Messages;

    using Moq;

    using NUnit.Framework;

    public class BattleshipAuditTests
    {
        #region Fields

        private AuditController auditController;

        private Mock<IAuditRepository> moqAuditRepository;

        private Mock<IMessagePublisher> moqMessagePublisher;

        private IEnumerable<Audit> auditLogs;

        private DateTime timestamp;

        #endregion

        #region Methods

        [SetUp]
        public void Setup()
        {
            this.timestamp = DateTime.Now;
            this.auditLogs = new List<Audit>
            {
               new Audit { AuditType = AuditType.Log, Message = "Log message", Username = "jakes@email.com", Timestamp = this.timestamp },
               new Audit { AuditType = AuditType.Error, Message = "Error message", Username = "jakes@email.com" , Timestamp = this.timestamp },
               new Audit { AuditType = AuditType.Content, Message = "Content content", Username = "jakes@email.com", Timestamp = this.timestamp }
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
            this.moqAuditRepository.Setup(q => q.GetAuditMessages()).Returns(Task.FromResult(this.auditLogs));

            // Act
            var result = await this.moqAuditRepository.Object.GetAuditMessages();

            // Assert
            Assert.AreEqual(this.auditLogs, result);
        }

        [Test]
        public async Task GetAuditLog_WhenGivenAType_ReturnAuditLogsByType()
        {
            // Arrange
            this.moqAuditRepository.Reset();
            this.moqAuditRepository.Setup(q => q.GetAuditMessagesByAuditType(AuditType.Log))
                                    .Returns(Task.FromResult(this.auditLogs.Where(q => q.AuditType == AuditType.Log)));

            // Act
            var result = await this.moqAuditRepository.Object.GetAuditMessagesByAuditType(AuditType.Log);

            // Assert - return one or more
            Assert.GreaterOrEqual(1, result.Count());
        }

        #endregion
    }
}