using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Accounts.Entities;
using Accounts.Repository;

namespace Accounts.Services.Entity
{
    public sealed class UserEntityService
    {
        readonly IUserRepository _repository;

        public UserEntityService(IUserRepository repository)
        {
            _repository = repository;
        }

        ~UserEntityService()
        {
            _repository.Dispose();
        }

        public List<User> GetUsers(string clientID)
        {
            try
            {
                return _repository.Get(u => u.ClientID == clientID)
                                  .ToList();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public User GetUser(string clientID, int id)
        {
            try
            {
                return _repository.Get(u => u.ClientID == clientID && u.ID == id)
                                  .SingleOrDefault();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public User GetUser(string clientID, string username)
        {
            try
            {
                return _repository.Get(u => u.ClientID == clientID && u.Username == username)
                                  .SingleOrDefault();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<User> GetUser(string clientID, string username, string password)
        {
            try
            {
                var user = _repository.Get(u => u.ClientID == clientID &&
                                                u.Username == username &&
                                                u.Password == CalculateHash(password))
                                      .SingleOrDefault();
                user.LastLogin = DateTimeOffset.Now;
                _repository.Update(user);
                await _repository.SaveAsync();
                return user;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Create(User user)
        {
            try
            {
                if (user.ID > 0)
                    throw new InvalidOperationException();

                user.Password = CalculateHash(user.Password);
                await _repository.AddAsync(user);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Activate(string clientID, int id)
        {
            try
            {
                var user = GetUser(clientID, id);

                if (user == null)
                    throw new ArgumentNullException(nameof(user), "user not found.");

                user.Active = true;
                _repository.Update(user);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task ChangePassword(string clientID, int id, string password)
        {
            try
            {
                var user = GetUser(clientID, id);

                if (user == null)
                    throw new ArgumentNullException(nameof(user), "user not found.");

                user.Password = CalculateHash(password);
                _repository.Update(user);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Delete(User user)
        {
            try
            {
                _repository.Remove(user);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Delete(string clientID, int id)
        {
            try
            {
                _repository.Remove(u => u.ClientID == clientID && u.ID == id);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        string CalculateHash(string password)
        {
            try
            {
                SHA256 sHA256 = SHA256.Create();
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] outputBytes = sHA256.ComputeHash(inputBytes);
                return Convert.ToBase64String(outputBytes);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
