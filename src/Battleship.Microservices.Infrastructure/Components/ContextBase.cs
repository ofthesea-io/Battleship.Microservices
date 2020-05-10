namespace Battleship.Microservices.Core.Components
{
    using System;

    using Battleship.Microservices.Core.Messages;
    using Battleship.Microservices.Core.Utilities;
    using Battleship.Microservices.Infrastructure.Messages;

    using Microsoft.AspNetCore.Mvc;

    public abstract class ContextBase : ControllerBase
    {
        #region Fields

        private readonly IMessagePublisher messagePublisher;

        #endregion

        #region Constructors

        protected ContextBase(IMessagePublisher messagePublisher)
        {
            this.messagePublisher = messagePublisher;
        }

        #endregion

        #region Methods

        protected async void Log(string content, AuditType auditType = AuditType.Content)
        {
            throw new NotImplementedException();
        }

        protected async void Log(Exception exp, AuditType auditType = AuditType.Error)
        {
            throw new NotImplementedException();
        }

        protected async void Log(Exception exp, string content, AuditType auditType = AuditType.Error)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}