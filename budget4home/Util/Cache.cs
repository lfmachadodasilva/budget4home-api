using budget4home.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
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

    public interface ICache
    {
        Task<T> GetOrCreateAsync<T>(CacheKey key, Func<Task<T>> factory, uint expireInHours = 1);
        void Delete(CacheKey key);
        void Delete(string key);
    }

    public class Cache : ICache
    {
        private readonly IDistributedCache _cache;

        public Cache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public Task<T> GetOrCreateAsync<T>(CacheKey key, Func<Task<T>> factory, uint expireInHours = 1)
        {
            return _cache.GetOrCreateAsync(key, factory, expireInHours);
        }

        public void Delete(CacheKey key)
        {
            this.Delete(key.ToString());
        }

        public void Delete(string key)
        {
            _cache.Delete(key);
        }
    }
}
