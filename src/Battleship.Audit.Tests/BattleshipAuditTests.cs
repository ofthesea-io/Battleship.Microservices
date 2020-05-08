namespace Battleship.Audit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Audit.Controllers;
    using Battleship.Audit.Infrastructure;
    using Battleship.Microservices.Infrastructure.Messages;
    using Battleship.Microservices.Infrastructure.Models;
    using Battleship.Microservices.Infrastructure.Utilities;

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
               new Audit { AuditType = AuditType.Message, Message = "Message content", Username = "jakes@email.com", Timestamp = this.timestamp }
            };

            this.moqAuditRepository = new Mock<IAuditRepository>();
            this.moqMessagePublisher = new Mock<IMessagePublisher>();
            this.auditController = new AuditController(this.moqAuditRepository.Object, this.moqMessagePublisher.Object);
        }

        [Test]
        public async Task GetAuditLog_WhenGivenNothing_GetsAllLogs()
        {
            // Arrange
            this.moqAuditRepository.Reset();
            this.moqAuditRepository.Setup(q => q.GetAuditMessages()).Returns(Task.FromResult(this.auditLogs));

            // Act
            var result = await this.moqAuditRepository.Object.GetAuditMessages();

            // Assert
            Assert.AreEqual(this.auditLogs, result);
        }

        #endregion
    }
}