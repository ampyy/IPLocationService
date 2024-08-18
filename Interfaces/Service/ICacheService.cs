
namespace Location.Interfaces.Service.ICache
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration) where T : class;
    }
}

