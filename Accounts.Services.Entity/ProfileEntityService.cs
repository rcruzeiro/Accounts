using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Entities;
using Accounts.Repository;

namespace Accounts.Services.Entity
{
    public sealed class ProfileEntityService
    {
        readonly IProfileRepository _repository;

        public ProfileEntityService(IProfileRepository repository)
        {
            _repository = repository;
        }

        ~ProfileEntityService()
        {
            _repository.Dispose();
        }

        public List<Profile> GetProfiles(string clientID)
        {
            try
            {
                return _repository.Get(p => p.ClientID == clientID)
                                  .ToList();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public Profile GetProfile(string clientID, int id)
        {
            try
            {
                return _repository.Get(p => p.ClientID == clientID && p.ID == id)
                                  .SingleOrDefault();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Save(Profile profile)
        {
            try
            {
                if (profile.ID == default(int))
                    await _repository.AddAsync(profile);
                else
                    _repository.Update(profile);

                await _repository.SaveAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Delete(Profile profile)
        {
            try
            {
                _repository.Remove(profile);
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
    }
}
