namespace ProjectManagementSystem.Data;

public interface ISerialization
{
    string Serialize<T>(T data);
    T? Deserialize<T>(string content);
}
