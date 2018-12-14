using System;
using System.Threading.Tasks;
using Accounts.Adapter;
using Accounts.API.Messages.Profiles;
using Accounts.DI;
using Accounts.DTO;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Accounts.API.Controllers
{
    [Route("[controller]")]
    public class ProfilesController : BaseController
    {
        public ProfilesController(IConfiguration configuration)
            : base(configuration)
        { }
        /// <summary>
        /// Get all profiles.
        /// </summary>
        /// <returns>A list with all the profiles objects.</returns>
        /// <param name="client">Client identifier.</param>
        /// <response code="200">Get was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetProfilesResponse), 200)]
        [ProducesResponseType(typeof(GetProfilesResponse), 500)]
        [HttpGet]
        public ActionResult<GetProfilesResponse> Get([FromHeader]string client)
        {
            GetProfilesResponse response;
            string responseCode = $"GET_PROFILES_{client}";
            string cacheKey = responseCode;

            try
            {
                if (string.IsNullOrEmpty(client))
                    throw new InvalidOperationException("client cannot be null.");

                if (ExistsInCache(cacheKey))
                    response = GetCache<GetProfilesResponse>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetProfile(_configuration);
                    var profiles = factory.GetProfiles(client);
                    response = new GetProfilesResponse
                    {
                        StatusCode = "200"
                    };
                    profiles.ForEach(profile =>
                        response.Data.Add(profile.Adapt()));
                    SetCache(cacheKey, profiles, 10080);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new GetProfilesResponse
                {
                    StatusCode = "500"
                };
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Get the specified profile.
        /// </summary>
        /// <returns>The profile object.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="id">Profile identifier.</param>
        /// <response code="200">Get was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetProfileResponse), 200)]
        [ProducesResponseType(typeof(GetProfileResponse), 500)]
        [HttpGet("{id}")]
        public ActionResult<GetProfileResponse> Get([FromHeader]string client, [FromRoute]int id)
        {
            GetProfileResponse response;
            string responseCode = $"GET_PROFILE_{client}_{id}";
            string cacheKey = responseCode;

            try
            {
                if (string.IsNullOrEmpty(client))
                    throw new InvalidOperationException("client cannot be null.");

                if (ExistsInCache(cacheKey))
                    response = GetCache<GetProfileResponse>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetProfile(_configuration);
                    var profile = factory.GetProfile(client, id);
                    response = new GetProfileResponse
                    {
                        StatusCode = "200"
                    };
                    response.Data = profile.Adapt();
                    SetCache(cacheKey, response, 10080);
                    return Ok(response);
                }

                return response;
            }
            catch (Exception ex)
            {
                response = new GetProfileResponse
                {
                    StatusCode = "500"
                };
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Create a new profile.
        /// </summary>
        /// <returns>The create status code.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="request">New profile info.</param>
        /// <response code="200">Create was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(NewProfileResponse), 200)]
        [ProducesResponseType(typeof(NewProfileResponse), 500)]
        [HttpPost]
        public async Task<ActionResult<NewProfileResponse>> Post([FromHeader]string client, [FromBody]CreateProfileRequest request)
        {
            NewProfileResponse response = new NewProfileResponse();
            string responseCode = $"CREATE_PROFILE_{client}";

            try
            {
                if (string.IsNullOrEmpty(client))
                    throw new InvalidOperationException("client cannot be null.");

                if (client != request.ClientID)
                    throw new InvalidOperationException("invalid client ID in request object.");

                var dto = new ProfileDTO
                {
                    ClientID = client,
                    Title = request.Title,
                    Description = request.Description,
                    Active = true
                };
                request.GrantIDs.ForEach(id => dto.Grants.Add(new GrantDTO { ID = id }));
                var factory = AccountsFactory.Instance.GetSaveProfile(_configuration);
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
        /// <summary>
        /// Delete the specified profile.
        /// </summary>
        /// <returns>The delete status code.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="id">Profile identifier.</param>
        /// <response code="200">Delete was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(NewProfileResponse), 200)]
        [ProducesResponseType(typeof(NewProfileResponse), 500)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<NewProfileResponse>> Delete([FromHeader]string client, [FromRoute]int id)
        {
            NewProfileResponse response = new NewProfileResponse();
            string responseCode = $"DELETE_PROFILE_{client}_{id}";

            try
            {
                if (string.IsNullOrEmpty(client))
                    throw new InvalidOperationException("client cannot be null.");

                var factory = AccountsFactory.Instance.GetProfile(_configuration);
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
