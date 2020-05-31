namespace Battleship.Audit.Controllers
{
    using System.Threading.Tasks;

    using Battleship.Infrastructure.Core.Models;

    using Microsoft.AspNetCore.Mvc;

    public interface IAuditController
    {
        #region Methods

        Task<ActionResult> GetAuditContent();

        Task<ActionResult> SaveAuditContent(Audit audit);

        Task<ActionResult> GetAuditContentByAuditTypeHourRange(int auditTypeId, int hours = 0);

        #endregion
    }
}