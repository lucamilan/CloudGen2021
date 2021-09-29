using ServiceStack.Redis;
using System;

namespace Dapr.Cqrs.Core.Counters
{
    public class RedisCountersService
    {
        private readonly IRedisClientsManager _clientsManager;

        public RedisCountersService(IRedisClientsManager clientsManager)
        {
            _clientsManager = clientsManager;
        }

        public long Incr(string key, int incrBy = 1)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (_clientsManager.GetClient() is not IRedisNativeClient redis)
                throw new ArgumentNullException(nameof(redis));
            using (redis)
            {
                return redis.IncrBy(key, incrBy);
            }
        }

        public long Read(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (_clientsManager.GetClient() is not IRedisNativeClient redis)
                throw new ArgumentNullException(nameof(redis));
            using (redis)
            {
                return redis.Exists(key) > 0 ? Incr(key, 0) : 0;
            }
        }
    }
}