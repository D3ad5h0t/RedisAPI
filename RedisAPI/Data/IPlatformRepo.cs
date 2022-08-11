using RedisAPI.Models;

namespace RedisAPI.Data
{
    public interface IPlatformRepo
    {
        void CreatePlatform(Platform platform);

        Platform? GetPlatformById(string id);

        IEnumerable<Platform> GetAllPlatforms();
    }
}