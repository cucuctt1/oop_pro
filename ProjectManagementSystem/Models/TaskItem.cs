namespace ProjectManagementSystem.Models;

public class TaskItem
{
    private string _taskId;
    private string _title;
    private string _description;
    private EnumStatus _status;
    private Employee? _assignee;

    public TaskItem()
    {
        _taskId = string.Empty;
        _title = string.Empty;
        _description = string.Empty;
        _status = EnumStatus.Pending;
        _assignee = null;
    }

    public string TaskId
    {
        get { return _taskId; }
        set { _taskId = value == null ? string.Empty : value; }
    }

    public string Title
    {
        get { return _title; }
        set { _title = value == null ? string.Empty : value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value == null ? string.Empty : value; }
    }

    public EnumStatus Status
    {
        get { return _status; }
        set { _status = value; }
    }

    public Employee? Assignee
    {
        get { return _assignee; }
        set { _assignee = value; }
    }
}
