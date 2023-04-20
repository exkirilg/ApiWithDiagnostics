namespace ApiWithDiagnostics.Services;

public class HttpRequests : IHttpRequests
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpRequests(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<T> Fetch<T>(string httpClientName, string requestUri)
    {
        var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var result = await httpClient.GetAsync(requestUri);

        result.EnsureSuccessStatusCode();
        
        return (await result.Content.ReadFromJsonAsync<T>())!;
    }

    public async Task<string> FetchAsString(string httpClientName, string requestUri)
    {
        var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var result = await httpClient.GetAsync(requestUri);

        result.EnsureSuccessStatusCode();

        return await result.Content.ReadAsStringAsync();
    }
}
