using System;
using System.Security.Cryptography;
using System.Text;
using Accounts.API.Filters;
using Accounts.API.Messages;
using Accounts.DTO;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Accounts.API.Controllers
{
    [ClientFilter]
    [Route("[controller]")]
    public class AuthorizeController : CachedController
    {
        const int daysInCache = 365; //a full year

        public AuthorizeController(IConfiguration configuration, [FromServices]RedisDTO redis)
            : base(configuration, redis)
        { }
        /// <summary>
        /// Authorize an application.
        /// </summary>
        /// <returns>The authorize token as well the refresh token.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="request">The application to be authorized.</param>
        /// <response code="200">Authorize was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(AuthorizeResponse), 200)]
        [ProducesResponseType(typeof(AuthorizeResponse), 500)]
        [HttpPost]
        public ActionResult<AuthorizeResponse> Post([FromHeader]string client, [FromBody]AuthorizeRequest request)
        {
            AuthorizeResponse response = new AuthorizeResponse();
            string responseCode = $"AUTH_{client}_{request.ApplicationID}";
            string cacheKey = responseCode;

            try
            {
                if (ExistsInCache(cacheKey))
                    response.Data = GetFromCache<AuthorizeDTO>(cacheKey);
                else
                {
                    response.Data = new AuthorizeDTO
                    {
                        Token = GenerateToken(client, request.ApplicationID),
                        ExpiresOn = DateTimeOffset.Now.AddDays(daysInCache)
                    };
                    SetToCache(cacheKey,
                             response.Data,
                             (int)TimeSpan.FromDays(daysInCache).TotalSeconds);
                }

                response.StatusCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// De-authorize an application.
        /// </summary>
        /// <returns>The de-authorize status code.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="request">The application to be de-authorized.</param>
        /// <response code="200">De-authorize was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DeAuthorizeResponse), 200)]
        [ProducesResponseType(typeof(DeAuthorizeResponse), 500)]
        [HttpDelete]
        public ActionResult<DeAuthorizeResponse> Delete([FromHeader]string client, [FromBody]AuthorizeRequest request)
        {
            DeAuthorizeResponse response = new DeAuthorizeResponse();
            string cacheKey = $"AUTH_{client}_{request.ApplicationID}";

            try
            {
                if (ExistsInCache(cacheKey))
                    RemoveFromCache(cacheKey);

                response.StatusCode = 200;
                response.Data = $"Application {request.ApplicationID} is no longer authorized.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, $"DEAUTHORIZE_{client}_{request.ApplicationID}"));
                return StatusCode(500, response);
            }
        }

        string GenerateToken(string client, string applicationID)
        {
            try
            {
                SHA256 sHA256 = SHA256.Create();
                byte[] input = Encoding.ASCII.GetBytes(
                    $"AUTH_{applicationID}_FOR_{client}_IN_{DateTimeOffset.Now}");
                byte[] output = sHA256.ComputeHash(input);
                return Convert.ToBase64String(output);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
