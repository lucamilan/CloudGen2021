using Dapr.Cqrs.Api.Read.Mappers;
using Dapr.Cqrs.Api.Read.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace Dapr.Cqrs.Api.Read.Controllers
{
    [ApiController]
    [Route("search-data")]
    public class SearchDataController : ControllerBase
    {
        [HttpGet("{plantKey}/{locationKey}/{tagKey}")]
        public async Task<IActionResult> GetDirectoriesOrFilesAsync([FromServices] AzureBlobManagement azureBlobManagement, string plantKey, string locationKey, string tagKey, [FromQuery] String prefix)
        {
            var filter = $"{plantKey}/{locationKey}/{tagKey}/";

            if(prefix != null) 
                filter += prefix.Trim('/') + "/";

            var list = await azureBlobManagement.GetListOfDirectoriesAsync(filter);
            return new JsonResult(list);
        }
    }
}