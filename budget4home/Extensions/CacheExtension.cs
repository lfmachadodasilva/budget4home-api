using budget4home.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace budget4home.Extensions
{
    public static class CacheExtension
    {
        public static IServiceCollection SetupCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = configuration.GetConnectionString("Redis");
            });

            return services;
        }    

        private static readonly ConcurrentDictionary<string, Timer> _keys = new();

        private static void TimerCallback(Object o)
        {
            string toRemove = (string)o;

            _keys.TryRemove(toRemove, out var timer);

            Console.WriteLine("Remove: " + toRemove + (timer != null).ToString());
            if (timer != null)
            {
                timer.Dispose();
            }
        }

        public static async Task<T> GetOrCreateAsync<T>(
            this IDistributedCache cache,
            CacheKey key,
            Func<Task<T>> factory,
            uint expireInHours = 1)
        {
            try
            {
                var itemSerialize = await cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(itemSerialize))
                {
                    await cache.RefreshAsync(key);
                    return JsonSerializer.Deserialize<T>(itemSerialize);
                }
            }
            catch
            {
                // ignore missing/corrupt value
            }

            var item = await factory();

            try
            {
                var itemSerialized = JsonSerializer.Serialize(item);
                _keys.TryAdd(key, new Timer(TimerCallback, key.ToString(), expireInHours * 3600000, Timeout.Infinite));
                Console.WriteLine("Add: " + key.ToString());
                await cache.SetStringAsync(key, itemSerialized, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = new TimeSpan(0, (int)expireInHours, 0, 0)
                });
            }
            catch
            {
                // ignore missing/corrupt value
            }

            return item;
        }

        public static void Delete(this IDistributedCache cache, CacheKey key)
        {
            cache.Delete(key.ToString());
        }

        public static void Delete(this IDistributedCache cache, string key)
        {
            var keysToRemove = _keys.ToList();
            for (var i = 0; i < keysToRemove.Count; i++)
            {
                var keyToRemove = keysToRemove[i].Key;

                //Console.WriteLine(keyToRemove);
                if (!keyToRemove.Contains(key))
                    return;

                try
                {
                    TimerCallback(keyToRemove);
                    cache.Remove(keyToRemove);
                }
                catch
                {
                    // just ignore
                }
            }
        }
    }
}
