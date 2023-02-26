using Microsoft.AspNetCore.Mvc;

namespace Api_Versionning_Core_Net.Controllers._1._0
{
    [ApiVersion("1.0")]
    [Obsolete("Attention cette Api est expiré !")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var result = await Task.FromResult("Hello version 1.0");

            return Ok(result);
        }
    }
}
