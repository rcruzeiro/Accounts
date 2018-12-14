using System;
using Core.Framework.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Accounts.API.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;
        readonly ICacheService _cache;

        protected BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
            _cache = CacheFactory.Instance.Redis(
                _configuration.GetValue<string>("Redis:Endpoint"),
                _configuration.GetValue<int>("Redis:Database"));
            _cache.Expires = TimeSpan.FromMinutes(
                _configuration.GetValue<int>("Redis:Expires"));
        }

        protected bool ExistsInCache(string key)
        {
            try
            {
                return _cache.Exists(key);
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected T GetCache<T>(string key)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(_cache.Get(key));
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected void SetCache(string key, object value, int? minutesToExpire = null)
        {
            try
            {
                _cache.Set(key, JsonConvert.SerializeObject(value), minutesToExpire);
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected void RemoveCache(string key)
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
    }
}
