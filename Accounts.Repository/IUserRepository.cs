using Accounts.Entities;
using Core.Framework.Repository;

namespace Accounts.Repository
{
    public interface IUserRepository : IRepositoryAsync<User>
    { }
}
