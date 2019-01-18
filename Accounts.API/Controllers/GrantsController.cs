using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Adapter;
using Accounts.API.Filters;
using Accounts.API.Messages.Grants;
using Accounts.DI;
using Accounts.DTO;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Accounts.API.Controllers
{
    [ClientFilter]
    [Route("[controller]")]
    public class GrantsController : CachedController
    {
        public GrantsController(IConfiguration configuration, [FromServices]RedisDTO redis)
            : base(configuration, redis)
        { }
        /// <summary>
        /// Get all grants.
        /// </summary>
        /// <returns>A list with all the grants objects.</returns>
        /// <param name="client">Client identifier.</param>
        /// <response code="200">Get was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetGrantsResponse), 200)]
        [ProducesResponseType(typeof(GetGrantsResponse), 500)]
        [HttpGet]
        public ActionResult<GetGrantsResponse> Get([FromHeader]string client)
        {
            GetGrantsResponse response = new GetGrantsResponse();
            string responseCode = $"GET_GRANTS_{client}";
            string cacheKey = responseCode;

            try
            {
                if (ExistsInCache(cacheKey))
                    response.Data = GetFromCache<List<GrantDTO>>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetGrant(_configuration);
                    var grants = factory.GetGrants(client);
                    grants.ForEach(grant =>
                        response.Data.Add(grant.Adapt()));
                    SetToCache(cacheKey, response.Data);
                }

                response.StatusCode = 200;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Get the specified grant.
        /// </summary>
        /// <returns>The grant object.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="id">Grant identifier.</param>
        /// <response code="200">Get was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetGrantResponse), 200)]
        [ProducesResponseType(typeof(GetGrantResponse), 500)]
        [HttpGet("{id}")]
        public ActionResult<GetGrantResponse> Get([FromHeader]string client, [FromRoute]int id)
        {
            GetGrantResponse response = new GetGrantResponse();
            string responseCode = $"GET_GRANT_{client}_{id}";
            string cacheKey = responseCode;

            try
            {
                if (ExistsInCache(cacheKey))
                    response.Data = GetFromCache<GrantDTO>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetGrant(_configuration);
                    var grant = factory.GetGrant(client, id);
                    response.Data = grant.Adapt();
                    SetToCache(cacheKey, response.Data);
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
        /// Get the specified grant by code.
        /// </summary>
        /// <returns>The grant object.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="code">Grant identifier.</param>
        /// <response code="200">Get was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetGrantResponse), 200)]
        [ProducesResponseType(typeof(GetGrantResponse), 500)]
        [HttpGet("code/{code}")]
        ActionResult<GetGrantResponse> Get([FromHeader]string client, [FromRoute]string code)
        {
            GetGrantResponse response = new GetGrantResponse();
            string responseCode = $"GET_GRANT_{client}_{code}";
            string cacheKey = responseCode;

            try
            {
                if (ExistsInCache(cacheKey))
                    response.Data = GetFromCache<GrantDTO>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetGrant(_configuration);
                    var grant = factory.GetGrant(client, code);
                    response.Data = grant.Adapt();
                    SetToCache(cacheKey, response.Data);
                }

                response.StatusCode = 200;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Create a new grant.
        /// </summary>
        /// <returns>The create status code.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="request">New grant info.</param>
        /// <response code="200">Create was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(NewGrantResponse), 200)]
        [ProducesResponseType(typeof(NewGrantResponse), 500)]
        [HttpPost]
        public async Task<ActionResult<NewGrantResponse>> Post([FromHeader]string client, [FromBody]CreateGrantRequest request)
        {
            NewGrantResponse response = new NewGrantResponse();
            string responseCode = $"CREATE_GRANT_{client}";

            try
            {
                var dto = new GrantDTO
                {
                    ClientID = client,
                    Code = request.Code,
                    Title = request.Title,
                    Description = request.Description,
                    Action = request.Action,
                    Active = true
                };
                var factory = AccountsFactory.Instance.GetGrant(_configuration);
                await factory.Save(dto.Adapt());
                response.StatusCode = 200;
                response.Data = "Grant created with success.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Delete the specified grant.
        /// </summary>
        /// <returns>The delete status code.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="id">Grant identifier.</param>
        /// <response code="200">Delete was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(NewGrantResponse), 200)]
        [ProducesResponseType(typeof(NewGrantResponse), 500)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<NewGrantResponse>> Delete([FromHeader]string client, [FromRoute]int id)
        {
            NewGrantResponse response = new NewGrantResponse();
            string responseCode = $"DELETE_GRANT_{client}_{id}";

            try
            {
                var factory = AccountsFactory.Instance.GetGrant(_configuration);
                await factory.Delete(client, id);
                response.StatusCode = 200;
                response.Data = "Grant deleted with success.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
    }
}
