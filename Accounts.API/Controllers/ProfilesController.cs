using System;
using System.Threading.Tasks;
using Accounts.Adapter;
using Accounts.API.Filters;
using Accounts.API.Messages.Profiles;
using Accounts.DI;
using Accounts.DTO;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Accounts.API.Controllers
{
    [ClientFilter]
    [Route("[controller]")]
    public class ProfilesController : CachedController
    {
        public ProfilesController(IConfiguration configuration, [FromServices]RedisDTO redis)
             : base(configuration, redis)
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
            GetProfilesResponse response = new GetProfilesResponse();
            string responseCode = $"GET_PROFILES_{client}";
            string cacheKey = responseCode;

            try
            {
                if (ExistsInCache(cacheKey))
                    response = GetFromCache<GetProfilesResponse>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetProfile(_configuration);
                    var profiles = factory.GetProfiles(client);
                    profiles.ForEach(profile =>
                        response.Data.Add(profile.Adapt()));
                    SetToCache(cacheKey, response);
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
            GetProfileResponse response = new GetProfileResponse();
            string responseCode = $"GET_PROFILE_{client}_{id}";
            string cacheKey = responseCode;

            try
            {
                if (ExistsInCache(cacheKey))
                    response.Data = GetFromCache<ProfileDTO>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetProfile(_configuration);
                    var profile = factory.GetProfile(client, id);
                    response.Data = profile.Adapt();
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
                var dto = new ProfileDTO
                {
                    ClientID = client,
                    Title = request.Title,
                    Description = request.Description,
                    Active = true
                };
                var factory = AccountsFactory.Instance.GetProfile(_configuration);
                await factory.Save(dto.Adapt());
                response.StatusCode = 200;
                response.Data = "Profile created with success.";
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
                var factory = AccountsFactory.Instance.GetProfile(_configuration);
                await factory.Delete(client, id);
                response.StatusCode = 200;
                response.Data = "Profile deleted with success.";
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
        /// Recreates the grants list in the specified profile.
        /// </summary>
        /// <returns>The put status code.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="id">Profile identifier.</param>
        /// <param name="request">The grant ids.</param>
        /// <response code="200">Update was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(NewProfileResponse), 200)]
        [ProducesResponseType(typeof(NewProfileResponse), 500)]
        [HttpPut("{id}/grants")]
        public async Task<ActionResult<NewProfileResponse>> AddGrant([FromHeader]string client, [FromRoute]int id, [FromBody]AddGrantProfileRequest request)
        {
            NewProfileResponse response = new NewProfileResponse();
            string responseCode = $"ADD_GRANT_{client}_PROFILE_{id}";

            try
            {
                var factory = AccountsFactory.Instance.GetSaveProfileGrants(_configuration);
                await factory.Save(client, id, request.GrantIDs);
                response.StatusCode = 200;
                response.Data = "Grant(s) added with success.";
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
