using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Data;

public sealed class SystemContext
{
    private static readonly object _lock = new object();
    private static SystemContext? _instance;

    private readonly List<Project> _projects;
    private readonly List<Employee> _employees;
    private Project? _currentProject;

    private SystemContext()
    {
        _projects = new List<Project>();
        _employees = new List<Employee>();
        _currentProject = null;
        SeedDefaultEmployees();
    }

    public static SystemContext Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SystemContext();
                    }
                }
            }

            return _instance;
        }
    }

    public Project? CurrentProject
    {
        get { return _currentProject; }
        set { _currentProject = value; }
    }

    public void AddProject(Project project)
    {
        if (project == null)
        {
            return;
        }

        _projects.Add(project);
    }

    public bool RemoveProjectById(string projectId)
    {
        for (int i = 0; i < _projects.Count; i++)
        {
            if (_projects[i].ProjectId == projectId)
            {
                _projects.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public List<Project> ListProjects()
    {
        List<Project> copiedList = new List<Project>();

        for (int i = 0; i < _projects.Count; i++)
        {
            copiedList.Add(_projects[i]);
        }

        return copiedList;
    }

    public void SetProjects(List<Project> projects)
    {
        _projects.Clear();

        if (projects == null)
        {
            return;
        }

        for (int i = 0; i < projects.Count; i++)
        {
            _projects.Add(projects[i]);
        }
    }

    public List<Employee> GetEmployees()
    {
        List<Employee> copiedList = new List<Employee>();

        for (int i = 0; i < _employees.Count; i++)
        {
            copiedList.Add(_employees[i]);
        }

        return copiedList;
    }

    private void SeedDefaultEmployees()
    {
        if (_employees.Count > 0)
        {
            return;
        }

        _employees.Add(new ProjectLeader("PL001", "Alice Nguyen", 33, "alice@company.com"));
        _employees.Add(new ProjectLeader("PL002", "Bob Tran", 36, "bob@company.com"));
        _employees.Add(new Employee("EM001", "Charlie Le", 28, "charlie@company.com"));
        _employees.Add(new Employee("EM002", "Diana Pham", 27, "diana@company.com"));
    }
}
