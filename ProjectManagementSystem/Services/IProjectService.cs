using ProjectManagementSystem.Builders;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services;

public interface IProjectService
{
    bool CreateProject(ProjectBuilder builder, out string message);
    List<Project> GetProjects();
    bool DeleteProject(string projectId, out string message);
    bool UpdateProjectStatus(string projectId, EnumStatus status, out string message);
    bool CreateTask(string projectId, string title, string description, EnumStatus status, Employee assignee, out string message);
    List<TaskItem> GetTasksByProjectId(string projectId);
    List<Employee> GetAvailableLeaders();
    List<Employee> GetAvailableEmployees();
    List<EnumStatus> GetStatusOptions();
}
