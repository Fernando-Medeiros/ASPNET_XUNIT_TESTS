using Newtonsoft.Json;
using System.Text;

namespace Test.Setup;

internal sealed record Header(string Key, string Value);

internal enum EMediaType : byte
{
    ApplicationJson,
    MultipartFormData,
    TextPlain,
}

internal sealed class RequestFixture(string endpoint)
{
    public string Endpoint { get; } = endpoint;
    public StringContent? Payload { get; private set; }
    public List<Header> Headers { get; private set; } = [];

    public RequestFixture JsonContent(object data, EMediaType media)
    {
        string json = JsonConvert.SerializeObject(data);

        string type = media switch
        {
            EMediaType.ApplicationJson => "application/json",
            EMediaType.MultipartFormData => "multipart/form-data",
            EMediaType.TextPlain => "text/plain",
            _ => ""
        };

        Payload = new StringContent(json, Encoding.UTF8, type);

        return this;
    }

    public RequestFixture AuthorizationHeader()
    {
        string token = string.Empty;

        Headers.Add(new("Authorization", $"Bearer {token}"));

        return this;
    }

    public HttpRequestMessage RequestMessage(HttpMethod method)
    {
        HttpRequestMessage request = new(method, Endpoint) { Content = Payload };

        Headers.ForEach(
            (x) => request.Headers.Add($"{x.Key}", $"{x.Value}"));

        return request;
    }

    public static async Task<T?> Deserialize<T>(HttpResponseMessage response) =>

        System.Text.Json.JsonSerializer.Deserialize<T>(
                json: await response.Content.ReadAsStringAsync(),
                options: new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
}
