using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Accounts.Adapter;
using Accounts.API.Filters;
using Accounts.API.Messages;
using Accounts.API.Messages.User;
using Accounts.API.Services;
using Accounts.DI;
using Accounts.DTO;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Accounts.API.Controllers
{
    [ClientFilter]
    [Route("[controller]")]
    public class AuthenticateController : CachedController
    {
        const int daysInCache = 182; //6 months

        public AuthenticateController(IConfiguration configuration, [FromServices]RedisDTO redis)
            : base(configuration, redis)
        { }
        /// <summary>
        /// Authenticate an user.
        /// </summary>
        /// <returns>The authenticated user info.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="request">Info of the application and user to authenticate.</param>
        /// <param name="signin">Siginin service.</param>
        /// <param name="token">Token service.</param>
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
        public async Task<ActionResult<AuthenticateUserResponse>> Post([FromHeader]string client,
                                                                       [FromServices]SigninConfiguration signin,
                                                                       [FromServices]TokenDTO token,
                                                                       [FromBody]AuthenticateUserRequest request)
        {
            AuthorizeResponse authResponse = new AuthorizeResponse();
            AuthenticateUserResponse response = new AuthenticateUserResponse();
            string responseCode = $"AUTHENTICATE_USER_{client}_{request.Username}";
            string authorizeCacheKey = $"AUTH_{client}_{request.ApplicationID}";

            try
            {
                // validate that the application is authorized
                if (!ExistsInCache(authorizeCacheKey))
                {
                    response.StatusCode = 401;
                    response.Messages.Add(ResponseMessage.Create(responseCode, "Application not authorized for this action."));
                    return StatusCode(401, response);
                }
                //validate if the application token is valid
                authResponse.Data = GetFromCache<AuthorizeDTO>(authorizeCacheKey);

                if (authResponse.Data.Token != request.Token)
                {
                    response.StatusCode = 400;
                    response.Messages.Add(ResponseMessage.Create(responseCode, "Invalid application token"));
                    return BadRequest(response);
                }
                //authenticate
                var factory = AccountsFactory.Instance.GetUser(_configuration);
                var user = await factory.GetUser(client, request.Username, request.Password);
                UserDTO dto = user.Adapt();

                #region JWT configuration
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.Username, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
                    });
                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = token.Issuer,
                    Audience = token.Audience,
                    SigningCredentials = signin.Credentials,
                    Subject = identity,
                    NotBefore = DateTime.Now,
                    Expires = DateTime.Now + TimeSpan.FromSeconds(token.Seconds)
                });
                dto.Token = handler.WriteToken(securityToken);
                #endregion

                response.StatusCode = 200;
                response.Data = dto;
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
