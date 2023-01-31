using System.Net.Http.Headers;

namespace Shared.RestApiClient;
/// <summary>
/// Rest api client extension
/// </summary>
public interface IApiClient
{
    Task<string?> GetAsync(string absoluteUriPath, string jsonData);
    Task<string?> PostAsync(string absoluteUriPath, string jsonData);
    Task<string?> DeleteAsync(string absoluteUriPath, string jsonData);
}
public class ApiClient: IApiClient
{
    public async Task<string?> GetAsync(string absoluteUriPath, string jsonData)
    {
        var httpRequest = new HttpRequestMessage();
        httpRequest.Method = HttpMethod.Get;
        string? result = await SendAsyncRequest(absoluteUriPath,jsonData,httpRequest);
        return result;
    }

    public async Task<string?> PostAsync(string absoluteUriPath, string jsonData)
    {
        var httpRequest = new HttpRequestMessage();
        httpRequest.Method = HttpMethod.Post;
        string? result = await SendAsyncRequest(absoluteUriPath, jsonData,httpRequest);
        return result;
    }

    public async Task<string?> DeleteAsync(string absoluteUriPath, string jsonData)
    {
        var httpRequest = new HttpRequestMessage();
        httpRequest.Method = HttpMethod.Delete;
        string? result = await SendAsyncRequest(absoluteUriPath, jsonData,httpRequest);
        return result;
    }
    
    private async Task<string?> SendAsyncRequest(string absoluteUriPath, string jsonData, HttpRequestMessage httpRequest)
    {
        var url = absoluteUriPath;
        httpRequest.RequestUri = new Uri(url);
        httpRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");
        httpRequest.Headers.TryAddWithoutValidation("accept", "application/json");
        /*httpRequest.Headers.TryAddWithoutValidation("Authorization", $"Bearer {}");*/
        
        if (!string.IsNullOrWhiteSpace(jsonData))
        {
            httpRequest.Content = new StringContent(jsonData);
            httpRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        }
        
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

        var httpClient = new HttpClient(clientHandler);
        HttpResponseMessage result = await httpClient.SendAsync(httpRequest);
        string? response = null;
        if (result.IsSuccessStatusCode)
        {
            response = await result.Content.ReadAsStringAsync();
        }
        
        return response;
    }
}
