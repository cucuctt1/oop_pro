using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Data;

public sealed class DataStorage
{
    private static readonly object _lock = new object();
    private static DataStorage? _instance;

    private readonly string _filePath;
    private readonly ISerialization _serialization;

    private DataStorage()
        : this(new JsonSerialization())
    {
    }

    private DataStorage(ISerialization serialization)
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
        _serialization = serialization;
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
        string json = _serialization.Serialize(projects);
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
            List<Project>? projects = _serialization.Deserialize<List<Project>>(json);

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
