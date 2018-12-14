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
        /// <summary>
        /// Service health check.
        /// </summary>
        /// <returns>The "pong" string.</returns>
        /// <response code="200">The service is up and running.</response>
        [ProducesResponseType(typeof(string), 200)]
        [HttpGet("ping")]
        public ActionResult<string> Get()
        {
            return Ok("pong");
        }
        /// <summary>
        /// Clear all service cache.
        /// </summary>
        /// <returns>The response code.</returns>
        /// <response code="200">The clear was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ClearCacheResponse), 200)]
        [ProducesResponseType(typeof(ClearCacheResponse), 500)]
        [HttpDelete("cache")]
        public ActionResult<ClearCacheResponse> Delete()
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
