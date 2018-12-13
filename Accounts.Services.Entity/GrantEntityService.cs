using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Entities;
using Accounts.Repository;

namespace Accounts.Services.Entity
{
    public sealed class GrantEntityService
    {
        readonly IGrantRepository _repository;

        public GrantEntityService(IGrantRepository repository)
        {
            _repository = repository;
        }

        ~GrantEntityService()
        {
            _repository.Dispose();
        }

        public List<Grant> GetGrants(string clientID)
        {
            try
            {
                return _repository.Get(p => p.ClientID == clientID)
                                  .ToList();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public Grant GetGrant(string clientID, int id)
        {
            try
            {
                return _repository.Get(p => p.ClientID == clientID && p.ID == id)
                                  .SingleOrDefault();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public Grant GetGrant(string clientID, string code)
        {
            try
            {
                return _repository.Get(p => p.ClientID == clientID && p.Code == code)
                                  .SingleOrDefault();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Save(Grant permission)
        {
            try
            {
                if (permission.ID == default(int))
                    await _repository.AddAsync(permission);
                else
                    _repository.Update(permission);

                await _repository.SaveAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Delete(Grant permission)
        {
            try
            {
                _repository.Remove(permission);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Delete(string clientID, int id)
        {
            try
            {
                _repository.Remove(p => p.ClientID == clientID && p.ID == id);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Delete(string clientID, string code)
        {
            try
            {
                _repository.Remove(p => p.ClientID == clientID && p.Code == code);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
