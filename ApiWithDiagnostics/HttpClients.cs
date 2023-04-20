namespace ApiWithDiagnostics;

public static class HttpClients
{
    public const string WeatherHttpClientName = "Weather";
    public const string ArchiveWeatherHttpClientName = "ArchiveWeather";
    public const string CatsFactsHttpClientName = "CatsFacts";
    public const string PostmanEchoHttpClientName = "PostmanEcho";

    public static void ConfigureHttpClients(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient(WeatherHttpClientName, httpClient =>
        {
            httpClient.BaseAddress = new Uri(@"https://api.open-meteo.com/v1/");
        });

        builder.Services.AddHttpClient(ArchiveWeatherHttpClientName, httpClient =>
        {
            httpClient.BaseAddress = new Uri(@"https://archive-api.open-meteo.com/v1/");
        });

        builder.Services.AddHttpClient(CatsFactsHttpClientName, httpClient =>
        {
            httpClient.BaseAddress = new Uri(@"https://cat-fact.herokuapp.com/");
        });

        builder.Services.AddHttpClient(PostmanEchoHttpClientName, httpClient =>
        {
            httpClient.BaseAddress = new Uri(@"https://postman-echo.com/");
        });
    }
}
