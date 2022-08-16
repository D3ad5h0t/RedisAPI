using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data
{
    public class RedisPlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisPlatformRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentOutOfRangeException(nameof(platform));
            }

            var db = _redis.GetDatabase();

            var serialPlatform = JsonSerializer.Serialize(platform);

            db.StringSet(platform.Id, serialPlatform);
            db.SetAdd("PlatformSet", serialPlatform);
        }

        public IEnumerable<Platform?>? GetAllPlatforms()
        {
            var db = _redis.GetDatabase();

            var completeSet = db.SetMembers("PlatformSet");

            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<Platform>(val)).ToList();

                return obj;
            }

            return null;
        }

        public Platform? GetPlatformById(string id)
        {
            var db = _redis.GetDatabase();

            var platform = db.StringGet(id);

            if (!string.IsNullOrWhiteSpace(platform))
            {
                return JsonSerializer.Deserialize<Platform>(platform);
            }

            return null;
        }
    }
}