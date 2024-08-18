using System.Diagnostics;
using Location.Interfaces.Services;
using Location.Models;

namespace Location.Services.LocationLogic
{
    public class ProviderCallerLogic : IProviderCallerLogic
    {
        private readonly IProviderSelectorLogic _providerSelector;

        public ProviderCallerLogic(IProviderSelectorLogic providerSelector)
        {
            _providerSelector = providerSelector;
        }

        /// <summary>
        /// Calls the API of the selected provider, updates metrics, and returns the JSON response.
        /// </summary>
        /// <param name="provider">The provider containing API details.</param>
        /// <param name="ipAddress">The IP address to query.</param>
        /// <returns>The JSON response from the provider API.</returns>
        public async Task<string> CallProviderApiAsync(Provider provider, string ipAddress)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new ArgumentException("IP address cannot be null or empty.", nameof(ipAddress));
            }

            try
            {
                string responseContent = string.Empty;
                string requestUrl = string.Format(provider.Url, ipAddress, provider.Token);

                // Measure the response time
                var stopwatch = Stopwatch.StartNew();
                using (var httpClient = new HttpClient())
                {
                    // Send the request and get the response
                    var response = await httpClient.GetAsync(requestUrl);
                    response.EnsureSuccessStatusCode();

                    // Read and return the response content as a string
                    responseContent = await response.Content.ReadAsStringAsync();
                }

                stopwatch.Stop();
                double responseTime = stopwatch.Elapsed.TotalMilliseconds;
                // Update metrics for the provider
                await UpdateMetricsAsync(provider.Name, false, responseTime);
                return responseContent;
            }
            catch (Exception ex)
            {
                // Update metrics for the provider with error
                await UpdateMetricsAsync(provider.Name, true, 0);
                throw new ApplicationException("Error calling the provider API", ex);
            }
        }

        private async Task UpdateMetricsAsync(string providerName, bool isError, double responseTime)
        {
            await _providerSelector.UpdateMetricsAsync(providerName, isError, responseTime);
        }
    }
}
