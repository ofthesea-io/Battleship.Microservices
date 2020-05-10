namespace Battleship.Audit.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Battleship.Audit.Infrastructure;
    using Battleship.Microservices.Core.Messages;
    using Battleship.Microservices.Core.Models;
    using Battleship.Microservices.Core.Utilities;
    using Battleship.Microservices.Infrastructure.Messages;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        #region Fields

        private readonly IAuditRepository auditRepository;

        private readonly IMessagePublisher messagePublisher;

        #endregion

        #region Constructors

        public AuditController(IAuditRepository auditRepository, IMessagePublisher messagePublisher)
        {
            this.auditRepository = auditRepository;
            this.messagePublisher = messagePublisher;
        }

        #endregion

        #region Methods

        [HttpPost]
        [Route("SaveAuditMessage")]
        public async Task<ActionResult> SaveAuditMessage([FromBody] Audit audit)
        {
            try
            {
                if (audit == null)
                    return this.StatusCode(StatusCodes.Status500InternalServerError);

                await this.auditRepository.SaveAuditMessage(audit.AuditType, audit.Message, audit.Username);
                return this.Ok();
            }
            catch (Exception e)
            {
                string message = $"Battleship.Player: {e.StackTrace}";
                await this.messagePublisher.PublishAuditLogMessageAsync(AuditType.Error, message);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetAuditLogs")]
        public async Task<ActionResult> GetAuditMessages()
        {
            try
            {
                var result = await this.auditRepository.GetAuditMessages();
                if (result == null) return this.BadRequest(false);

                var json = JsonConvert.SerializeObject(result);
                return this.Ok(json);
            }
            catch (Exception e)
            {
                string message = $"Battleship.Player: {e.StackTrace}";
                await this.messagePublisher.PublishAuditLogMessageAsync(AuditType.Error, message);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}