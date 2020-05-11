namespace Battleship.Audit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Audit.Infrastructure;
    using Battleship.Microservices.Core.Components;
    using Battleship.Microservices.Core.Messages;
    using Battleship.Microservices.Core.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ContextBase
    {
        #region Fields

        private readonly IAuditRepository auditRepository;

        #endregion

        #region Constructors

        public AuditController(IAuditRepository auditRepository, IMessagePublisher messagePublisher)
            : base(messagePublisher)
        {
            this.auditRepository = auditRepository;
        }

        #endregion

        #region Methods

        [HttpPost]
        [Route("SaveAuditContent")]
        public async Task<ActionResult> SaveAuditMessage([FromBody] Audit audit)
        {
            try
            {
                if (audit == null)
                    return this.StatusCode(StatusCodes.Status500InternalServerError);

                await this.auditRepository.SaveAuditContent(audit.Content, audit.AuditType, DateTime.Now);
                return this.Ok();
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetAuditLogs")]
        public async Task<ActionResult> GetAuditMessages()
        {
            try
            {
                IEnumerable<Audit> result = await this.auditRepository.GetAuditContent();
                if (result == null) return this.BadRequest(false);

                string json = JsonConvert.SerializeObject(result);
                return this.Ok(json);
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}