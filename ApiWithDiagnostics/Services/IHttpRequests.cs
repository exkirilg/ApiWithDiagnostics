namespace ApiWithDiagnostics.Services;

public interface IHttpRequests
{
    Task<T> Fetch<T>(string httpClientName, string requestUri);
    Task<string> FetchAsString(string httpClientName, string requestUri);
}
