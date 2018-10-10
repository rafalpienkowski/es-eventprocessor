using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace StatlerWaldorfCorp.EventProcessor.Location.Redis
{
    public class RedisLocationCache : ILocationCache
    {
        private ILogger _logger;
        private IConnectionMultiplexer _connection;

        public RedisLocationCache(ILogger<RedisLocationCache> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _connection = connectionMultiplexer;

            _logger.LogInformation($"Using redis location cache - {connectionMultiplexer.Configuration}");
        }

        public RedisLocationCache(ILogger<RedisLocationCache> logger, ConnectionMultiplexer connectionMultiplexer) : this(logger, (IConnectionMultiplexer)connectionMultiplexer)
        {    
        }

        public IList<MemberLocation> GetMemberLocations(Guid teamId)
        {
            IDatabase db = _connection.GetDatabase();
            var values = db.HashValues(teamId.ToString());

            return ConvertRedisValuesToLocationList(values);
        }

        private IList<MemberLocation> ConvertRedisValuesToLocationList(RedisValue[] values)
        {
            var memberLocations = new List<MemberLocation>();
            for (int i = 0; i < values.Length; i++)
            {
                var value = (string)values[i];
                var memberLocation = MemberLocation.FromJsonString(value);
                memberLocations.Add(memberLocation);
            }            

            return memberLocations;
        }

        public void Put(Guid teamId, MemberLocation memberLocation)
        {
            IDatabase db = _connection.GetDatabase();
            db.HashSet(teamId.ToString(), memberLocation.MemberId.ToString(), memberLocation.ToJsonString());
        }
    }
}