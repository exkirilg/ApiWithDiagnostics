namespace ApiWithDiagnostics;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        app.MapGet("current-weather/{latitude}-{longitude}", GetCurrentWeatherAsync);
        app.MapGet("random-cats-facts", GetRandomCatsFact);
        app.MapGet("postman-echo", GetPostmanEcho);
    }

    private static async Task<IResult> GetCurrentWeatherAsync(IHttpClientFactory httpClientFactory, float latitude, float longitude)
    {
        var httpClient = httpClientFactory.CreateClient(HttpClients.WeatherHttpClientName);

        string requestUri = $"forecast?latitude={latitude:0.0000}&longitude={longitude:0.0000}&current_weather=true";

        return await ProcessHttpRequest(httpClient, requestUri);
    }

    private static async Task<IResult> GetRandomCatsFact(IHttpClientFactory httpClientFactory, int amount = 1)
    {
        var httpClient = httpClientFactory.CreateClient(HttpClients.CatsFactsHttpClientName);

        string requestUri = $"facts/random?animal_type=cat&amount={amount}";

        return await ProcessHttpRequest(httpClient, requestUri);
    }

    private static async Task<IResult> GetPostmanEcho(IHttpClientFactory httpClientFactory)
    {
        var httpClient = httpClientFactory.CreateClient(HttpClients.PostmanEchoHttpClientName);

        return await ProcessHttpRequest(httpClient, "get?foo=bar");
    }

    private static async Task<IResult> ProcessHttpRequest(HttpClient httpClient, string requestUri)
    {
        try
        {
            string data = await FetchRequestedDataAsString(httpClient, requestUri);
            return Results.Ok(data);
        }
        catch
        {
            return Results.Problem(detail: "Unable to fetch data", statusCode: 500);
        }
    }

    private static async Task<string> FetchRequestedDataAsString(HttpClient httpClient, string requestUri)
    {
        var result = await httpClient.GetAsync(requestUri);
        result.EnsureSuccessStatusCode();
        return await result.Content.ReadAsStringAsync();
    }
}
