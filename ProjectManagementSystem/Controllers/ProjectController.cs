using ProjectManagementSystem.Builders;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers;

public class ProjectController
{
    private readonly IProjectService _projectService;

    public ProjectController()
    {
        _projectService = new ProjectService();
    }

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public bool CreateProject(
        string name,
        string description,
        DateTime startDate,
        DateTime endDate,
        EnumStatus status,
        Employee leader,
        out string message)
    {
        return CreateProject(name, description, startDate, endDate, status, leader, new List<Employee>(), out message);
    }

    public bool CreateProject(
        string name,
        string description,
        DateTime startDate,
        DateTime endDate,
        EnumStatus status,
        Employee leader,
        List<Employee> involvedEmployees,
        out string message)
    {
        ProjectBuilder builder = new ProjectBuilder();
        builder.SetName(name);
        builder.SetDescription(description);
        builder.SetStartDate(startDate);
        builder.SetEndDate(endDate);
        builder.SetStatus(status);
        builder.SetLeader(leader);
        builder.SetEmployees(involvedEmployees);

        return _projectService.CreateProject(builder, out message);
    }

    public List<Project> GetProjects()
    {
        return _projectService.GetProjects();
    }

    public bool DeleteProject(string projectId, out string message)
    {
        return _projectService.DeleteProject(projectId, out message);
    }

    public bool UpdateProjectStatus(string projectId, EnumStatus status, out string message)
    {
        return _projectService.UpdateProjectStatus(projectId, status, out message);
    }

    public bool CreateTask(string projectId, string title, string description, EnumStatus status, Employee assignee, out string message)
    {
        return _projectService.CreateTask(projectId, title, description, status, assignee, out message);
    }

    public List<TaskItem> GetTasksByProjectId(string projectId)
    {
        return _projectService.GetTasksByProjectId(projectId);
    }

    public List<Employee> GetLeaders()
    {
        return _projectService.GetAvailableLeaders();
    }

    public List<Employee> GetEmployees()
    {
        return _projectService.GetAvailableEmployees();
    }

    public List<EnumStatus> GetStatusOptions()
    {
        return _projectService.GetStatusOptions();
    }
}
