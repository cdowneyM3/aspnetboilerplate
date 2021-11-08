﻿using Abp.Runtime.Caching;
using System;

namespace Abp.RedisCache.Hybrid
{
    /// <summary>
    /// Defines a combination Redis / In Memory cache item
    /// </summary>
    public interface IAbpRedisHybridCache : ICache
    {
        /// <summary>
        /// Saves/Overrides an item in the cache by a key for mem cache only.
        /// Use one of the expire times at most (<paramref name="slidingExpireTime"/> or <paramref name="absoluteExpireTime"/>).
        /// If none of them is specified, then
        /// <see cref="DefaultAbsoluteExpireTime"/> will be used if it's not null. Othewise, <see cref="DefaultSlidingExpireTime"/>
        /// will be used.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="slidingExpireTime">Sliding expire time</param>
        /// <param name="absoluteExpireTime">Absolute expire time</param>
        void SetMemoryOnly(string key, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// Saves/Overrides an item in the cache by a key for mem cache only..
        /// </summary>
        /// <param name="key">Key</param>
        void RemoveMemoryOnly(string key);
    }
}
