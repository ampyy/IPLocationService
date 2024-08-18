
using Location.Models;

namespace Location.Interfaces.Services
{
    public interface IProviderSelectorLogic
    {
        Task<Provider> GetBestProviderAsync();
        Task UpdateMetricsAsync(string providerName, bool isError, double responseTime);
    }
}

