using System.Net.Http;

namespace AppForSEII2526.Web.API
{
    // Minimal partial declaration so project compiles while the source generator creates the full implementation.
    public partial class AppForSEII2526APIClient
    {
        private readonly string? _baseUrl;
        private readonly HttpClient _httpClient;

        public AppForSEII2526APIClient(string? baseUrl, HttpClient httpClient)
        {
            _baseUrl = baseUrl;
            _httpClient = httpClient ?? new HttpClient();
            if (!string.IsNullOrEmpty(_baseUrl))
            {
                try
                {
                    _httpClient.BaseAddress = new System.Uri(_baseUrl!);
                }
                catch
                {
                    // ignore invalid base url here; generator may add validation
                }
            }
        }
    }
}
