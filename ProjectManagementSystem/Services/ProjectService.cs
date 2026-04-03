using ProjectManagementSystem.Builders;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services;

public class ProjectService : IProjectService
{
    private readonly SystemContext _context;
    private readonly DataStorage _dataStorage;

    public ProjectService()
    {
        _context = SystemContext.Instance;
        _dataStorage = DataStorage.Instance;
    }

    public bool CreateProject(ProjectBuilder builder, out string message)
    {
        if (builder == null)
        {
            message = "Builder is required.";
            return false;
        }

        Project project = builder.Build();

        if (!ValidateProject(project, out message))
        {
            return false;
        }

        project.ProjectId = GenerateProjectId();

        if (project.Employees == null)
        {
            project.Employees = new List<Employee>();
        }

        if (project.Tasks == null)
        {
            project.Tasks = new List<TaskItem>();
        }

        NormalizeEmployees(project);

        _context.AddProject(project);

        try
        {
            _dataStorage.SaveData();
            message = "Project created successfully.";
            return true;
        }
        catch (Exception ex)
        {
            message = "Project created but failed to save data: " + ex.Message;
            return false;
        }
    }

    public List<Project> GetProjects()
    {
        return _context.ListProjects();
    }

    public bool DeleteProject(string projectId, out string message)
    {
        if (string.IsNullOrWhiteSpace(projectId))
        {
            message = "Project id is required.";
            return false;
        }

        bool removed = _context.RemoveProjectById(projectId);

        if (!removed)
        {
            message = "Project not found.";
            return false;
        }

        try
        {
            _dataStorage.SaveData();
            message = "Project deleted successfully.";
            return true;
        }
        catch (Exception ex)
        {
            message = "Project deleted but failed to save data: " + ex.Message;
            return false;
        }
    }

    public bool UpdateProjectStatus(string projectId, EnumStatus status, out string message)
    {
        if (string.IsNullOrWhiteSpace(projectId))
        {
            message = "Project id is required.";
            return false;
        }

        Project? project = FindProjectById(projectId);
        if (project == null)
        {
            message = "Project not found.";
            return false;
        }

        project.Status = status;

        try
        {
            _dataStorage.SaveData();
            message = "Project status updated successfully.";
            return true;
        }
        catch (Exception ex)
        {
            message = "Status updated but failed to save data: " + ex.Message;
            return false;
        }
    }

    public bool CreateTask(string projectId, string title, string description, EnumStatus status, Employee assignee, out string message)
    {
        if (string.IsNullOrWhiteSpace(projectId))
        {
            message = "Project id is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            message = "Task title is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            message = "Task description is required.";
            return false;
        }

        if (assignee == null)
        {
            message = "Assignee is required.";
            return false;
        }

        Project? project = FindProjectById(projectId);
        if (project == null)
        {
            message = "Project not found.";
            return false;
        }

        if (project.Employees == null)
        {
            project.Employees = new List<Employee>();
        }

        bool exists = false;
        for (int i = 0; i < project.Employees.Count; i++)
        {
            if (project.Employees[i].Id == assignee.Id)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
        {
            project.Employees.Add(assignee);
        }

        if (project.Tasks == null)
        {
            project.Tasks = new List<TaskItem>();
        }

        TaskItem task = new TaskItem();
        task.TaskId = GenerateTaskId(project);
        task.Title = title.Trim();
        task.Description = description.Trim();
        task.Status = status;
        task.Assignee = assignee;

        project.Tasks.Add(task);

        try
        {
            _dataStorage.SaveData();
            message = "Task created and assigned successfully.";
            return true;
        }
        catch (Exception ex)
        {
            message = "Task created but failed to save data: " + ex.Message;
            return false;
        }
    }

    public List<TaskItem> GetTasksByProjectId(string projectId)
    {
        List<TaskItem> copiedTasks = new List<TaskItem>();

        if (string.IsNullOrWhiteSpace(projectId))
        {
            return copiedTasks;
        }

        Project? project = FindProjectById(projectId);
        if (project == null)
        {
            return copiedTasks;
        }

        if (project.Tasks == null)
        {
            project.Tasks = new List<TaskItem>();
        }

        for (int i = 0; i < project.Tasks.Count; i++)
        {
            copiedTasks.Add(project.Tasks[i]);
        }

        return copiedTasks;
    }

    public List<Employee> GetAvailableLeaders()
    {
        List<Employee> allEmployees = _context.GetEmployees();
        List<Employee> leaders = new List<Employee>();

        for (int i = 0; i < allEmployees.Count; i++)
        {
            if (allEmployees[i] is ProjectLeader)
            {
                leaders.Add(allEmployees[i]);
            }
        }

        if (leaders.Count == 0)
        {
            for (int i = 0; i < allEmployees.Count; i++)
            {
                leaders.Add(allEmployees[i]);
            }
        }

        return leaders;
    }

    public List<Employee> GetAvailableEmployees()
    {
        return _context.GetEmployees();
    }

    public List<EnumStatus> GetStatusOptions()
    {
        List<EnumStatus> statuses = new List<EnumStatus>();
        statuses.Add(EnumStatus.Pending);
        statuses.Add(EnumStatus.OnGoing);
        statuses.Add(EnumStatus.Completed);
        statuses.Add(EnumStatus.Abandoned);
        return statuses;
    }

    private bool ValidateProject(Project project, out string message)
    {
        if (project == null)
        {
            message = "Project data is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(project.ProjectName))
        {
            message = "Project name is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(project.Description))
        {
            message = "Project description is required.";
            return false;
        }

        if (project.StartDate > project.EndDate)
        {
            message = "Start date must be earlier than or equal to end date.";
            return false;
        }

        if (project.Leader == null)
        {
            message = "Project leader is required.";
            return false;
        }

        message = string.Empty;
        return true;
    }

    private void NormalizeEmployees(Project project)
    {
        if (project.Employees == null)
        {
            project.Employees = new List<Employee>();
        }

        List<Employee> normalized = new List<Employee>();

        for (int i = 0; i < project.Employees.Count; i++)
        {
            Employee? employee = project.Employees[i];
            if (employee == null)
            {
                continue;
            }

            bool exists = false;
            for (int j = 0; j < normalized.Count; j++)
            {
                if (normalized[j].Id == employee.Id)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                normalized.Add(employee);
            }
        }

        project.Employees = normalized;

        if (project.Leader != null)
        {
            bool leaderExists = false;
            for (int i = 0; i < project.Employees.Count; i++)
            {
                if (project.Employees[i].Id == project.Leader.Id)
                {
                    leaderExists = true;
                    break;
                }
            }

            if (!leaderExists)
            {
                project.Employees.Add(project.Leader);
            }
        }
    }

    private Project? FindProjectById(string projectId)
    {
        List<Project> projects = _context.ListProjects();

        for (int i = 0; i < projects.Count; i++)
        {
            if (projects[i].ProjectId == projectId)
            {
                return projects[i];
            }
        }

        return null;
    }

    private string GenerateProjectId()
    {
        List<Project> projects = _context.ListProjects();
        int maxValue = 0;

        for (int i = 0; i < projects.Count; i++)
        {
            string currentId = projects[i].ProjectId;
            if (string.IsNullOrWhiteSpace(currentId))
            {
                continue;
            }

            if (!currentId.StartsWith("PRJ-"))
            {
                continue;
            }

            if (currentId.Length <= 4)
            {
                continue;
            }

            string numberPart = currentId.Substring(4);
            int parsed;
            if (int.TryParse(numberPart, out parsed))
            {
                if (parsed > maxValue)
                {
                    maxValue = parsed;
                }
            }
        }

        int nextValue = maxValue + 1;
        return "PRJ-" + nextValue.ToString("D4");
    }

    private string GenerateTaskId(Project project)
    {
        if (project.Tasks == null)
        {
            project.Tasks = new List<TaskItem>();
        }

        int maxValue = 0;

        for (int i = 0; i < project.Tasks.Count; i++)
        {
            string currentId = project.Tasks[i].TaskId;
            if (string.IsNullOrWhiteSpace(currentId))
            {
                continue;
            }

            if (!currentId.StartsWith("TSK-"))
            {
                continue;
            }

            if (currentId.Length <= 4)
            {
                continue;
            }

            string numberPart = currentId.Substring(4);
            int parsed;
            if (int.TryParse(numberPart, out parsed))
            {
                if (parsed > maxValue)
                {
                    maxValue = parsed;
                }
            }
        }

        int nextValue = maxValue + 1;
        return "TSK-" + nextValue.ToString("D4");
    }
}
