using System.Net;
using System.Net.Http.Headers;

namespace Project.Server.Services.Common.Models;
public record HttpApiResult<T>(bool isSuccessStatusCode, HttpStatusCode statusCode, string? content, T? result) : IDisposable {
    public HttpApiResult(HttpContentHeaders? contentHeaders, bool isSuccessStatusCode, HttpStatusCode statusCode, string? content, T? result)
        : this(isSuccessStatusCode, statusCode, content, result) {

        ContentHeaders = contentHeaders;
    }

    public HttpApiResult(HttpContentHeaders? contentHeaders, bool isSuccessStatusCode, HttpStatusCode statusCode, T? result)
        : this(isSuccessStatusCode, statusCode, string.Empty, result) {

        ContentHeaders = contentHeaders;
    }

    public HttpContentHeaders? ContentHeaders {
        get; private set;
    } = null;

    public bool IsSuccessStatusCode {
        get; private set;
    } = isSuccessStatusCode;

    public HttpStatusCode StatusCode {
        get; private set;
    } = statusCode;

    public string? Content {
        get; private set;
    } = content;

    public T? Result {
        get; private set;
    } = result;

    internal static HttpApiResult<T> Success(HttpContentHeaders? contentHeaders, HttpStatusCode statusCode, T? result) {
        return new HttpApiResult<T>(contentHeaders, true, statusCode, result);
    }

    internal static HttpApiResult<T> Failure(HttpContentHeaders? contentHeaders, HttpStatusCode statusCode, T? result, string? content = null) {
        return new HttpApiResult<T>(contentHeaders, false, statusCode, content, result);
    }

    public void Dispose() {
        if (Result is IDisposable disposable) {
            disposable.Dispose();
        }
    }
}