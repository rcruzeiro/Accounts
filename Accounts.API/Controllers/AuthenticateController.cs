using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Accounts.API.Messages;
using Accounts.API.Messages.User;
using Accounts.DI;
using Accounts.DTO;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Accounts.API.Controllers
{
    [Route("[controller]")]
    public class AuthenticateController : BaseController
    {
        const int daysInCache = 182; //6 months
        string authorizeCacheKey;
        const string defaultCacheKey = "AUTH_";

        public AuthenticateController(IConfiguration configuration)
            : base(configuration)
        {
            authorizeCacheKey = _configuration.GetValue<string>("Keys:AUTHORIZE");
        }
        /// <summary>
        /// Authenticate an user.
        /// </summary>
        /// <returns>The authenticated user info.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="request">Info of the application and user to authenticate.</param>
        /// <response code="200">The user was authenticated successfully.</response>
        /// <response code="400">Invalid application token.</response>
        /// <response code="401">Application not authorized for this action.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(AuthenticateUserResponse), 200)]
        [ProducesResponseType(typeof(AuthenticateUserResponse), 400)]
        [ProducesResponseType(typeof(AuthenticateUserResponse), 401)]
        [ProducesResponseType(typeof(AuthenticateUserResponse), 500)]
        [HttpPost]
        public async Task<ActionResult<AuthenticateUserResponse>> Post([FromHeader]string client, [FromBody]AuthenticateUserRequest request)
        {
            AuthenticateUserResponse response = new AuthenticateUserResponse();
            string responseCode = $"AUTHENTICATE_USER_{client}_{request.Username}";
            authorizeCacheKey = $"{authorizeCacheKey}{client}_{request.ApplicationID}";
            string cacheKey = $"{defaultCacheKey}{client}_{request.ApplicationID}";

            try
            {
                // validate that the application is authorized
                if (!ExistsInCache(authorizeCacheKey))
                {
                    response.StatusCode = "401";
                    response.Messages.Add(ResponseMessage.Create(responseCode, "Application not authorized for this action."));
                    return StatusCode(401, response);
                }
                //validate if the application token is valid
                AuthorizeResponse authorizeResponse = GetCache<AuthorizeResponse>(authorizeCacheKey);

                if (authorizeResponse.Data.Token != request.Token)
                {
                    response.StatusCode = "400";
                    response.Messages.Add(ResponseMessage.Create(responseCode, "Invalid application token"));
                    return BadRequest(response);
                }
                //if application authorized, create the user temporary login
                var factory = AccountsFactory.Instance.GetUser(_configuration);
                var user = await factory.GetUser(client, request.Username, request.Password);
                //as there are no dto adapt we need to guarantee that the coming user is valid
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                response.StatusCode = "200";
                response.Data = new AuthenticatedUserDTO
                {
                    ID = user.ID,
                    ClientID = user.ClientID,
                    Username = user.Username,
                    Name = user.Name,
                    ExpiresOn = DateTimeOffset.Now.AddDays(daysInCache),
                    AccessToken = GenerateToken(client, request.ApplicationID, user.ID)
                };
                SetCache(
                    string.Join('_', cacheKey, response.Data.AccessToken),
                    response,
                    (int)TimeSpan.FromDays(daysInCache).TotalSeconds);
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
        /// Validate if an access token is valid.
        /// </summary>
        /// <returns>True if the specified token is valid.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="request">The info for validate the token.</param>
        /// <response code="200">Return if the token exists or not.</response>
        /// <response code="400">Invalid access token.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ValidateAuthResponse), 200)]
        [ProducesResponseType(typeof(ValidateAuthResponse), 400)]
        [ProducesResponseType(typeof(ValidateAuthResponse), 500)]
        [HttpPost("validate")]
        public ActionResult<ValidateAuthResponse> Validate([FromHeader]string client, [FromBody]ValidateAuthRequest request)
        {
            ValidateAuthResponse response = new ValidateAuthResponse();
            string responseCode = $"VALIDATE_AUTH_{client}_{request.AccessToken}";
            string cacheKey = $"{defaultCacheKey}{client}_{request.ApplicationID}_{request.AccessToken}";

            try
            {
                if (!ExistsInCache(cacheKey))
                {
                    response.Data = new ValidTokenDTO { IsValid = false };
                    response.Messages.Add(ResponseMessage.Create(responseCode, "Token does not exists."));
                }
                else
                {
                    var authUser = GetCache<AuthenticateUserResponse>(cacheKey);

                    if (authUser.Data.AccessToken != request.AccessToken)
                    {
                        response.StatusCode = "400";
                        response.Messages.Add(ResponseMessage.Create(responseCode, "Invalid token."));
                        return BadRequest(response);
                    }

                    response.Data = new ValidTokenDTO { IsValid = true };
                }

                response.StatusCode = "200";
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
        /// Logout an user.
        /// </summary>
        /// <returns>The logout status code.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="request">Request info for the logout proccess.</param>
        /// <response code="200">The logout was successful.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(LogoutResponse), 200)]
        [ProducesResponseType(typeof(LogoutResponse), 500)]
        [HttpDelete]
        public ActionResult<LogoutResponse> Logout([FromHeader]string client, [FromBody]LogoutRequest request)
        {
            LogoutResponse response = new LogoutResponse();
            string responseCode = $"LOGOUT_{client}_{request.AccessToken}";
            string cacheKey = $"{defaultCacheKey}{client}_{request.ApplicationID}_{request.AccessToken}";

            try
            {
                if (ExistsInCache(cacheKey))
                    RemoveCache(cacheKey);

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

        string GenerateToken(string client, string applicationID, int id)
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] input = Encoding.ASCII.GetBytes(
                $"AUTH_{client}_{applicationID}_{id}_{DateTimeOffset.Now}");
            byte[] output = sHA256.ComputeHash(input);
            return Convert.ToBase64String(output);
        }
    }
}
