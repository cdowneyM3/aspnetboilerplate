using Abp.Dependency;
using Abp.Runtime.Caching;
using Abp.Runtime.Caching.Memory;
using Abp.Runtime.Caching.Redis;
using System;

namespace Abp.RedisCache.Hybrid.Runtime.Caching.Redis
{
    public class AbpRedisHybridCache : CacheBase, IAbpRedisHybridCache
    {
        private readonly AbpMemoryCache _memoryCache;
        private readonly AbpRedisCache _redisCache;
        private readonly IIocManager _iocManager;

        public AbpRedisHybridCache(IIocManager iocManager, string name) : base(name)
        {
            _memoryCache = new AbpMemoryCache(name)
            {
                Logger = Logger
            };
            _iocManager = iocManager;
            _redisCache = _iocManager.Resolve<AbpRedisCache>(new { name });
        }

        public override object GetOrDefault(string key)
        {

            this.Logger.Debug($"GetOrDefault|{this.Name}|{key}");

            return _memoryCache.GetOrDefault(key);
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            this.Logger.Debug($"Set|{this.Name}|{key}");

            //Making sure the memory expires after the Redis, to give time for the redis events to be processed.
            var memorySlidingExpireTime = slidingExpireTime?.Add(new TimeSpan(0, 1, 0)) ?? _redisCache.DefaultSlidingExpireTime.Add(new TimeSpan(0, 1, 0));

            _memoryCache.Set(key, value, memorySlidingExpireTime, absoluteExpireTime);

            _redisCache.Set(key, value, slidingExpireTime, absoluteExpireTime);
        }

        public override void Remove(string key)
        {
            this.Logger.Debug($"Remove|{this.Name}|{key}");
            _memoryCache.Remove(key);
            _redisCache.Remove(key);
        }

        public void RemoveMemoryOnly(string key)
        {
            this.Logger.Debug($"RemoveMemoryOnly|{this.Name}|{key}");
            _memoryCache.Remove(key);
        }

        public override void Clear()
        {
            this.Logger.Debug($"Clear|{this.Name}");
            _memoryCache.Clear();
            _redisCache.Clear();
        }


        public override void Dispose()
        {
            this.Logger.Debug($"Dispose|{this.Name}");
            _memoryCache.Dispose();
            _iocManager.Release(_redisCache);
            base.Dispose();
        }


        public void SetMemoryOnly(string key, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            this.Logger.Debug($"SetMemoryOnly|{this.Name}|{key}");
            _memoryCache.Remove(key);

            var value = _redisCache.GetOrDefault(key);

            if (value != null)
            {
                _memoryCache.Set(key, value, slidingExpireTime, absoluteExpireTime);
            }
        }
    }
}
