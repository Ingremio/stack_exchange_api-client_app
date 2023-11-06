using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Project.Server.Services.Common.Models;

namespace Project.Server.Services.Common;
public abstract class HttpApiService {
    private const string MediaType = "application/json";

    private static HttpClient _client = new HttpClient();

    private readonly JsonSerializerOptions? _jsonSerializationOptions;
    private readonly ILogger _logger;

    public const HttpStatusCode ToManyRequests = (HttpStatusCode)429;

    protected HttpApiService(ILogger logger, HttpClient client) {
        _logger = logger;
        _client = client;

        _jsonSerializationOptions = GetDefaultJsonSerializationOptions();
    }

    public ILogger Logger => _logger;

    public virtual async Task<HttpApiResult<MemoryStream>?> GetStream(string? endpoint, CancellationToken cancellationToken, Func<HttpRequestMessage, bool>? setHeaders = null) {
        return await GetStreamFromApi(endpoint, cancellationToken, setHeaders);
    }

    public virtual async Task<HttpApiResult<TResult>?> GetData<TResult>(string? endpoint, CancellationToken cancellationToken, Action<HttpRequestMessage>? setContent = null, Func<HttpRequestMessage, bool>? setHeaders = null)
        where TResult : class, new() {

        return await GetDataFromApi<TResult>(endpoint, cancellationToken, setContent, setHeaders);
    }

    public virtual async Task<HttpApiResult<TResult>?> PostData<TModel, TResult>(TModel model, string? endpoint, CancellationToken cancellationToken, Action<HttpRequestMessage>? setContent = null, Func<HttpRequestMessage, bool>? setHeaders = null)
        where TResult : class, new() {

        return await PostDataToApi<TModel, TResult>(model, endpoint, cancellationToken, setContent, setHeaders);
    }

    public virtual async Task<HttpApiResult<TResult>?> PutData<TModel, TResult>(TModel model, string? endpoint, CancellationToken cancellationToken, Action<HttpRequestMessage>? setContent = null, Func<HttpRequestMessage, bool>? setHeaders = null)
        where TResult : class, new() {

        return await PutDataToApi<TModel, TResult>(model, endpoint, cancellationToken, setContent, setHeaders);
    }

    public virtual async Task<HttpApiResult<TResult>?> DeleteData<TResult>(string? endpoint, CancellationToken cancellationToken, Func<HttpRequestMessage, bool>? setHeaders = null)
        where TResult : class, new() {

        return await DeleteDataFromApi<TResult>(endpoint, cancellationToken, setHeaders);
    }

    protected virtual JsonSerializerOptions GetDefaultJsonSerializationOptions() {
        return new(JsonSerializerDefaults.Web) {
            TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default,
        };
    }

    protected virtual async Task<HttpApiResult<MemoryStream>?> GetStreamFromApi(string? endpoint, CancellationToken cancellationToken, Func<HttpRequestMessage, bool>? setHeaders = null) {
        return await QueryDataApi(endpoint, HttpMethod.Get, cancellationToken, setHeaders: setHeaders);
    }

    protected virtual async Task<HttpApiResult<T>?> GetDataFromApi<T>(string? endpoint, CancellationToken cancellationToken, Action<HttpRequestMessage>? setContent = null, Func<HttpRequestMessage, bool>? setHeaders = null)
        where T : class, new() {

        return await QueryDataApi<T>(endpoint, HttpMethod.Get, cancellationToken, setContent: setContent, setHeaders: setHeaders);
    }

    protected virtual async Task<HttpApiResult<TResult>?> PostDataToApi<TModel, TResult>(TModel model, string? endpoint, CancellationToken cancellationToken, Action<HttpRequestMessage>? setContent = null, Func<HttpRequestMessage, bool>? setHeaders = null)
        where TResult : class, new() {

        var httpContent = PrepareContent(model);

        return await QueryDataApi<TResult>(endpoint, HttpMethod.Post, cancellationToken, httpContent, setContent, setHeaders);
    }

    protected virtual async Task<HttpApiResult<TResult>?> PutDataToApi<TModel, TResult>(TModel model, string? endpoint, CancellationToken cancellationToken, Action<HttpRequestMessage>? setContent = null, Func<HttpRequestMessage, bool>? setHeaders = null)
        where TResult : class, new() {

        var httpContent = PrepareContent(model);

        return await QueryDataApi<TResult>(endpoint, HttpMethod.Put, cancellationToken, httpContent, setContent, setHeaders);
    }

    protected virtual async Task<HttpApiResult<TResult>?> DeleteDataFromApi<TResult>(string? endpoint, CancellationToken cancellationToken, Func<HttpRequestMessage, bool>? setHeaders = null)
        where TResult : class, new() {

        return await QueryDataApi<TResult>(endpoint, HttpMethod.Delete, cancellationToken, setHeaders: setHeaders);
    }

    protected virtual void SetContent(HttpRequestMessage requestMessage, HttpContent? httpContent) {
        if (httpContent != null) {
            requestMessage.Content = httpContent;
        }
    }

    protected virtual void SetHeaders(HttpRequestMessage requestMessage) {
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
    }

    private HttpContent PrepareContent<TModel>(TModel model) {
        var data = JsonSerializer.Serialize(model, _jsonSerializationOptions);

        return new StringContent(data, Encoding.UTF8, MediaType);
    }

    private async Task<HttpApiResult<TResult>?> QueryDataApi<TResult>(string? endpoint, HttpMethod method, CancellationToken cancellationToken, HttpContent? httpContent = null, Action<HttpRequestMessage>? setContent = null, Func<HttpRequestMessage, bool>? setHeaders = null)
       where TResult : class, new() {

        using var result = await QueryDataApi(endpoint, method, cancellationToken, httpContent, setContent, setHeaders);

        if (result == null) {
            return null;
        }

        if (result.IsSuccessStatusCode) {
            return HttpApiResult<TResult>.Success(result.ContentHeaders, result.StatusCode, JsonDeserializationFromStream<TResult>(result.Result));
        }

        var jsonString = await StreamToStringAsync(result.Result);

        return HttpApiResult<TResult>.Failure(result.ContentHeaders, result.StatusCode, null, jsonString);
    }

    private async Task<HttpApiResult<MemoryStream>?> QueryDataApi(string? endpoint, HttpMethod method, CancellationToken cancellationToken, HttpContent? httpContent = null, Action<HttpRequestMessage>? setContent = null, Func<HttpRequestMessage, bool>? setHeaders = null) {
        try {
            MemoryStream stream = new MemoryStream();

            using var response = await SendRequest(endpoint, method, cancellationToken, httpContent, setContent, setHeaders);

            if (response == null) {
                return null;
            }

            await response.Content.CopyToAsync(stream);

            if (stream.CanSeek && stream.Length == stream.Position) {
                stream.Position = 0;
            }

            if (response.IsSuccessStatusCode) {
                return HttpApiResult<MemoryStream>.Success(response.Content.Headers, response.StatusCode, stream);
            }

            return HttpApiResult<MemoryStream>.Failure(response.Content.Headers, response.StatusCode, stream);
        } catch (Exception ex) {
            _logger.LogError(ex, ex.Message);
        }

        return null;
    }

    private async Task<HttpResponseMessage?> SendRequest(string? endpoint, HttpMethod method, CancellationToken cancellationToken, HttpContent? httpContent, Action<HttpRequestMessage>? setContent, Func<HttpRequestMessage, bool>? setHeaders) {
        using var requestMessage = new HttpRequestMessage(method, endpoint);

        SetContent(requestMessage, httpContent);

        if (setContent != null) {
            setContent(requestMessage);
        }

        SetHeaders(requestMessage);

        if (setHeaders != null && !setHeaders(requestMessage)) {
            return null;
        }

        return await _client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
    }

    private static T? JsonDeserializationFromStream<T>(Stream? stream) {
        if (stream == null || !stream.CanRead) {
            return default;
        }

        var searchResult = JsonSerializer.Deserialize<T>(stream);
        return searchResult;
    }

    private static async Task<string?> StreamToStringAsync(Stream? stream) {
        if (stream == null) {
            return null;
        }

        using var sr = new StreamReader(stream);
        return await sr.ReadToEndAsync();
    }
}