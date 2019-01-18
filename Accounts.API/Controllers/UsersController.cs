using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Adapter;
using Accounts.API.Filters;
using Accounts.API.Messages.User;
using Accounts.DI;
using Accounts.DTO;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Accounts.API.Controllers
{
    [ClientFilter]
    [Route("[controller]")]
    public class UsersController : CachedController
    {
        public UsersController(IConfiguration configuration, [FromServices]RedisDTO redis)
            : base(configuration, redis)
        { }
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>A list with all users objects.</returns>
        /// <param name="client">Client identifier.</param>
        /// <response code="200">Get was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetUsersResponse), 200)]
        [ProducesResponseType(typeof(GetUsersResponse), 500)]
        [HttpGet]
        public ActionResult<GetUsersResponse> Get([FromHeader]string client)
        {
            GetUsersResponse response = new GetUsersResponse();
            string responseCode = $"GET_USERS_{client}";
            string cacheKey = responseCode;

            try
            {
                if (ExistsInCache(cacheKey))
                    response.Data = GetFromCache<List<UserDTO>>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetUser(_configuration);
                    var users = factory.GetUsers(client);
                    users.ForEach(user =>
                        response.Data.Add(user.Adapt()));
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
        /// Get the specified user.
        /// </summary>
        /// <returns>The user object.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="id">User identifier.</param>
        /// <response code="200">Get was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetUserResponse), 200)]
        [ProducesResponseType(typeof(GetUserResponse), 500)]
        [HttpGet("{id}")]
        public ActionResult<GetUserResponse> Get([FromHeader]string client, [FromRoute]int id)
        {
            GetUserResponse response = new GetUserResponse();
            string responseCode = $"GET_USER_{client}_{id}";
            string cacheKey = responseCode;

            try
            {
                if (ExistsInCache(cacheKey))
                    response.Data = GetFromCache<UserDTO>(cacheKey);
                else
                {
                    var factory = AccountsFactory.Instance.GetUser(_configuration);
                    var user = factory.GetUser(client, id);
                    response.Data = user.Adapt();
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
        /// Create a new user.
        /// </summary>
        /// <returns>The create status code.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="request">New user info.</param>
        /// <response code="200">The create was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(NewUserResponse), 200)]
        [ProducesResponseType(typeof(NewUserResponse), 500)]
        [HttpPost]
        public async Task<ActionResult<NewUserResponse>> Post([FromHeader]string client, [FromBody]CreateUserRequest request)
        {
            NewUserResponse response = new NewUserResponse();
            string responseCode = $"CREATE_USER_{client}";

            try
            {
                UserDTO dto = new UserDTO
                {
                    ClientID = client,
                    Name = request.Name,
                    Email = request.Email,
                    Username = request.Username,
                    Password = request.Password,
                    LocationID = request.LocationID,
                    Active = true
                };
                var factory = AccountsFactory.Instance.GetUser(_configuration);
                await factory.Create(dto.Adapt());
                response.StatusCode = 200;
                response.Data = "User created with success.";
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
