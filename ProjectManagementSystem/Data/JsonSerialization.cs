using System.Text.Json;

namespace ProjectManagementSystem.Data;

public sealed class JsonSerialization : ISerialization
{
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonSerialization()
    {
        _jsonOptions = new JsonSerializerOptions();
        _jsonOptions.WriteIndented = true;
    }

    public string Serialize<T>(T data)
    {
        return JsonSerializer.Serialize(data, _jsonOptions);
    }

    public T? Deserialize<T>(string content)
    {
        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }
}
