using System;
using Accounts.DTO;
using Core.Framework.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Accounts.API.Controllers
{
    public abstract class CachedController : Controller
    {
        protected readonly IConfiguration _configuration;
        readonly ICacheService _cache;

        protected CachedController(IConfiguration configuration, [FromServices]RedisDTO redisConfiguration)
        {
            _configuration = configuration;

            #region Cache configuration
            _cache = CacheFactory.Instance.Redis(
                redisConfiguration.Endpoint,
                redisConfiguration.Database);
            _cache.Expires = TimeSpan.FromMinutes(
                redisConfiguration.Expires);
            #endregion
        }

        #region Cache Members
        protected bool ExistsInCache(string key)
        {
            try
            {
                return _cache.Exists(key);
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected T GetFromCache<T>(string key)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(_cache.Get(key));
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected void SetToCache(string key, object value, int? secondsToExpire = null)
        {
            try
            {
                _cache.Set(key, JsonConvert.SerializeObject(value), secondsToExpire);
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected void RemoveFromCache(string key)
        {
            try
            {
                _cache.Remove(key);
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected void ClearCache()
        {
            try
            {
                _cache.Clear();
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion
    }
}
