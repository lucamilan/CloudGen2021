using Dapr.Cqrs.Api.Read.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dapr.Cqrs.Api.Read.Controllers
{
    [ApiController]
    [Route("raw-data")]
    public class RawDataController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRawAsync([FromServices] AzureBlobManagement azureBlobManagement, Guid id)
        {
            var blob = await azureBlobManagement.DownloadContentAsync(id);

            if (blob is null)
            {
                return NotFound(id);
            }

            return new JsonResult(blob);
        }
    }
}