namespace ProjectManagementSystem.Models;

public class Project
{
    private string _projectId;
    private string _projectName;
    private string _description;
    private DateTime _startDate;
    private DateTime _endDate;
    private EnumStatus _status;
    private Employee? _leader;
    private List<Employee> _employees;
    private List<TaskItem> _tasks;

    public Project()
    {
        _projectId = string.Empty;
        _projectName = string.Empty;
        _description = string.Empty;
        _startDate = DateTime.Today;
        _endDate = DateTime.Today;
        _status = EnumStatus.Pending;
        _leader = null;
        _employees = new List<Employee>();
        _tasks = new List<TaskItem>();
    }

    public string ProjectId
    {
        get { return _projectId; }
        set { _projectId = value == null ? string.Empty : value; }
    }

    public string ProjectName
    {
        get { return _projectName; }
        set { _projectName = value == null ? string.Empty : value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value == null ? string.Empty : value; }
    }

    public DateTime StartDate
    {
        get { return _startDate; }
        set { _startDate = value; }
    }

    public DateTime EndDate
    {
        get { return _endDate; }
        set { _endDate = value; }
    }

    public EnumStatus Status
    {
        get { return _status; }
        set { _status = value; }
    }

    public Employee? Leader
    {
        get { return _leader; }
        set { _leader = value; }
    }

    public List<Employee> Employees
    {
        get { return _employees; }
        set { _employees = value == null ? new List<Employee>() : value; }
    }

    public List<TaskItem> Tasks
    {
        get { return _tasks; }
        set { _tasks = value == null ? new List<TaskItem>() : value; }
    }
}
