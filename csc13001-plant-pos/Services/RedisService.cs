using StackExchange.Redis;

namespace csc13001_plant_pos.Services;

public class RedisService
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public RedisService(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _db = _redis.GetDatabase();
    }

    public async Task SetValueAsync(string key, string value, int expirationMinutes = 5)
    {
        await _db.StringSetAsync(key, value, TimeSpan.FromMinutes(expirationMinutes));
    }

    public async Task<string?> GetValueAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }

    public async Task DeleteKeyAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }
}
