using System.Text.Json;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Data;

public sealed class DataStorage
{
    private static readonly object _lock = new object();
    private static DataStorage? _instance;

    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    private DataStorage()
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
        _jsonOptions = new JsonSerializerOptions();
        _jsonOptions.WriteIndented = true;
    }

    public static DataStorage Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DataStorage();
                    }
                }
            }

            return _instance;
        }
    }

    public string DataFilePath
    {
        get { return _filePath; }
    }

    public void SaveData()
    {
        List<Project> projects = SystemContext.Instance.ListProjects();
        string json = JsonSerializer.Serialize(projects, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }

    public void LoadData()
    {
        if (!File.Exists(_filePath))
        {
            SystemContext.Instance.SetProjects(new List<Project>());
            return;
        }

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            SystemContext.Instance.SetProjects(new List<Project>());
            return;
        }

        try
        {
            List<Project>? projects = JsonSerializer.Deserialize<List<Project>>(json, _jsonOptions);

            if (projects == null)
            {
                projects = new List<Project>();
            }

            SystemContext.Instance.SetProjects(projects);
        }
        catch
        {
            SystemContext.Instance.SetProjects(new List<Project>());
        }
    }
}
