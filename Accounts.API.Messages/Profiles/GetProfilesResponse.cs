using System.Collections.Generic;
using Accounts.DTO;
using Core.Framework.API.Messages;

namespace Accounts.API.Messages.Profiles
{
    public class GetProfilesResponse : BaseResponse<List<ProfileDTO>>
    {
        public GetProfilesResponse()
        {
            Data = new List<ProfileDTO>();
        }
    }
}
