using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace budget4home.Util
{
    public class CacheKey
    {
        private const char SegmentSeparator = ':';
        private readonly IList<string> _segments;

        public CacheKey(params object[] segments)
        {
            GuardNotNull(segments, nameof(segments));

            _segments = new List<string>();
            foreach (var segment in segments)
            {
                Add(segment);
            }
        }

        public CacheKey Add(string segment)
        {
            GuardNotNull(segment, nameof(segment));

            var validated = ValidateSegment(segment);
            _segments.Add(validated);

            return this;
        }

        public CacheKey Add<T>(T segment)
        {
            GuardNotNull(segment, nameof(segment));

            return Add(segment.ToString());
        }

        public static implicit operator string(CacheKey key)
        {
            return key.ToString();
        }

        public override string ToString()
        {
            return string.Join(SegmentSeparator.ToString(), _segments);
        }

        private static string ValidateSegment(string segment)
        {
            if (string.IsNullOrWhiteSpace(segment))
            {
                throw new ArgumentException("Cache key segments must not be empty", nameof(segment));
            }

            var trimmed = segment.Trim(null); // null == whitespace

            return trimmed;
        }

        private static T GuardNotNull<T>(T parameter, string name)
        {
            if (parameter == null)
            {
                throw new ArgumentException("Cache key segments must not be null", name);
            }

            return parameter;
        }
    }

    public static class Cache
    {
        private static readonly ConcurrentBag<string> _keys = new ConcurrentBag<string>();

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
                _keys.Add(key);
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
            var keysToRemove = _keys.ToList();
            for (var i = 0; i < keysToRemove.Count; i++)
            {
                var keyToRemove = keysToRemove[i];

                //Console.WriteLine(keyToRemove);
                if (!keyToRemove.Contains(key))
                    return;

                try
                {
                    _keys.TryTake(out keyToRemove);
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
