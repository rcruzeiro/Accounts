using System;
using Accounts.API.Messages;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Accounts.API.Controllers
{
    [Route("[controller]")]
    public class HealthController : BaseController
    {
        public HealthController(IConfiguration configuration)
            : base(configuration)
        { }

        [HttpGet("ping")]
        public ActionResult<string> Get()
        {
            return Ok("pong");
        }

        [HttpDelete("cache")]
        public ActionResult<ClearCacheResponse> Put()
        {
            ClearCacheResponse response = new ClearCacheResponse();
            string responseCode = "CLEAR_CACHE";

            try
            {
                ClearCache();
                response.StatusCode = "200";
                response.Data = response.StatusCode;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = "500";
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
    }
}
