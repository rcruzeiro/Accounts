using System;
using System.Threading.Tasks;
using Accounts.Adapter;
using Accounts.API.Messages.Grants;
using Accounts.DI;
using Accounts.DTO;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Accounts.API.Controllers
{
    [Route("[controller]")]
    public class GrantsController : BaseController
    {
        public GrantsController(IConfiguration configuration)
            : base(configuration)
        { }

        [HttpGet]
        public ActionResult<GetGrantsResponse> Get([FromHeader]string client)
        {
            GetGrantsResponse response;
            string responseCode = $"GET_PERMISSIONS_{client}";
            string cacheKey = responseCode;

            try
            {
                if (ExistsInCache(cacheKey))
                    response = GetCache<GetGrantsResponse>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetGrant(_configuration);
                    var grants = factory.GetGrants(client);
                    response = new GetGrantsResponse
                    {
                        StatusCode = "200"
                    };
                    grants.ForEach(grant =>
                        response.Data.Add(grant.Adapt()));
                    SetCache(cacheKey, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new GetGrantsResponse
                {
                    StatusCode = "500"
                };
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, responseCode);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<GetGrantResponse> Get([FromHeader]string client, [FromRoute]int id)
        {
            GetGrantResponse response;
            string responseCode = $"GET_PERMISSION_{client}_{id}";
            string cacheKey = responseCode;

            try
            {
                if (ExistsInCache(cacheKey))
                    response = GetCache<GetGrantResponse>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetGrant(_configuration);
                    var grant = factory.GetGrant(client, id);
                    response = new GetGrantResponse
                    {
                        StatusCode = "200",
                        Data = grant.Adapt()
                    };
                    SetCache(cacheKey, response);
                }

                return Ok(response);

            }
            catch (Exception ex)
            {
                response = new GetGrantResponse
                {
                    StatusCode = "500"
                };
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }

        [HttpGet("{code}")]
        public ActionResult<GetGrantResponse> Get([FromHeader]string client, [FromRoute]string code)
        {
            GetGrantResponse response;
            string responseCode = $"GET_PERMISSION_{client}_{code}";
            string cacheKey = responseCode;

            try
            {
                if (ExistsInCache(cacheKey))
                    response = GetCache<GetGrantResponse>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetGrant(_configuration);
                    var grant = factory.GetGrant(client, code);
                    response = new GetGrantResponse
                    {
                        StatusCode = "200",
                        Data = grant.Adapt()
                    };
                    SetCache(cacheKey, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new GetGrantResponse
                {
                    StatusCode = "500"
                };
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<NewGrantResponse>> Post([FromHeader]string client, [FromBody]CreateGrantRequest request)
        {
            NewGrantResponse response = new NewGrantResponse();
            string responseCode = $"CREATE_PERMISSION_{client}";

            try
            {
                if (client != request.ClientID)
                    throw new InvalidOperationException("invalid client ID in request object.");

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

        [HttpDelete("{id}")]
        public async Task<ActionResult<NewGrantResponse>> Delete([FromHeader]string client, [FromRoute]int id)
        {
            NewGrantResponse response = new NewGrantResponse();
            string responseCode = $"DELETE_PERMISSION_{client}_{id}";

            try
            {
                var factory = AccountsFactory.Instance.GetGrant(_configuration);
                await factory.Delete(client, id);
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
