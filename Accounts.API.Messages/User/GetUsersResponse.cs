using System.Collections.Generic;
using Accounts.DTO;
using Core.Framework.API.Messages;

namespace Accounts.API.Messages.User
{
    public class GetUsersResponse : BaseResponse<List<UserDTO>>
    {
        public GetUsersResponse()
        {
            Data = new List<UserDTO>();
        }
    }
}
