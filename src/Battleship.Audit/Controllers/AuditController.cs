namespace Battleship.Audit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Audit.Infrastructure;
    using Battleship.Microservices.Core.Components;
    using Battleship.Microservices.Core.Messages;
    using Battleship.Microservices.Core.Models;
    using Battleship.Microservices.Core.Utilities;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ContextBase, IAuditController
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
        public async Task<ActionResult> SaveAuditContent([FromBody] Audit audit)
        {
            try
            {
                if (audit == null)
                    return this.StatusCode(StatusCodes.Status500InternalServerError);

                await this.auditRepository.SaveAuditContent(audit.Content, audit.AuditTypeId, DateTime.Now);
                return this.Ok();
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetAuditContent")]
        public async Task<ActionResult> GetAuditContent()
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

        [HttpGet]
        [Route("GetAuditContentByAuditTypeHourRange")]
        public async Task<ActionResult> GetAuditContentByAuditTypeHourRange(int auditType, int hours)
        {
            try
            {
                IEnumerable<Audit> result = await this.auditRepository.GetAuditContentByAuditTypeHourRange((AuditType)auditType, hours);
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

        [HttpGet]
        public string Get()
        {
            return "Audit Log API started.";
        }


        #endregion
    }
}