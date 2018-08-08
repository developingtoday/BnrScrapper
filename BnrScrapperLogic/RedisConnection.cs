using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace BnrScrapperLogic
{
    public class RedisConnection
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection =
                "";
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static ConnectionMultiplexer Connection => lazyConnection.Value;
    }
}
