using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Forms;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        if (ShouldRunSelfTest(args))
        {
            RunSelfVerification();
            return;
        }

        ApplicationConfiguration.Initialize();
        DataStorage.Instance.LoadData();
        Application.ApplicationExit += OnApplicationExit;
        Application.Run(new MainForm());
    }

    private static void OnApplicationExit(object? sender, EventArgs e)
    {
        DataStorage.Instance.SaveData();
    }

    private static bool ShouldRunSelfTest(string[] args)
    {
        if (args == null)
        {
            return false;
        }

        for (int i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], "--selftest", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static void RunSelfVerification()
    {
        DataStorage storage = DataStorage.Instance;
        SystemContext context = SystemContext.Instance;
        string reportFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "selftest-result.txt");
        List<string> reportLines = new List<string>();

        if (File.Exists(storage.DataFilePath))
        {
            File.Delete(storage.DataFilePath);
        }

        context.SetProjects(new List<Project>());

        ProjectController controller = new ProjectController();
        List<Employee> leaders = controller.GetLeaders();
        List<Employee> allEmployees = controller.GetEmployees();

        if (leaders.Count == 0)
        {
            reportLines.Add("SelfTest: No leaders available.");
            File.WriteAllLines(reportFilePath, reportLines);
            return;
        }

        string createMessage;
        List<Employee> involvedEmployees = new List<Employee>();
        for (int i = 0; i < allEmployees.Count; i++)
        {
            involvedEmployees.Add(allEmployees[i]);
            if (involvedEmployees.Count >= 2)
            {
                break;
            }
        }

        bool createValid = controller.CreateProject(
            "Website Revamp",
            "Initial project setup",
            new DateTime(2026, 4, 3),
            new DateTime(2026, 5, 10),
            EnumStatus.OnGoing,
            leaders[0],
            involvedEmployees,
            out createMessage);

        int countAfterCreate = controller.GetProjects().Count;

        string createdProjectId = string.Empty;
        List<Project> createdProjects = controller.GetProjects();
        if (createdProjects.Count > 0)
        {
            createdProjectId = createdProjects[0].ProjectId;
        }

        string statusUpdateMessage;
        bool statusUpdated = controller.UpdateProjectStatus(createdProjectId, EnumStatus.Completed, out statusUpdateMessage);

        int assignedEmployeeCount = 0;
        List<Project> projectsAfterCreate = controller.GetProjects();
        if (projectsAfterCreate.Count > 0)
        {
            Project firstProject = projectsAfterCreate[0];
            if (firstProject.Employees != null)
            {
                assignedEmployeeCount = firstProject.Employees.Count;
            }
        }

        string taskCreateMessage;
        bool taskCreated = controller.CreateTask(
            createdProjectId,
            "Setup CI Pipeline",
            "Configure build and deployment workflow",
            EnumStatus.OnGoing,
            leaders[0],
            out taskCreateMessage);

        int taskCountAfterCreate = controller.GetTasksByProjectId(createdProjectId).Count;

        string invalidDateMessage;
        bool invalidDateAccepted = controller.CreateProject(
            "Invalid Date Project",
            "This should fail",
            new DateTime(2026, 6, 1),
            new DateTime(2026, 5, 1),
            EnumStatus.Pending,
            leaders[0],
            out invalidDateMessage);

        storage.SaveData();
        int countBeforeReload = controller.GetProjects().Count;

        context.SetProjects(new List<Project>());
        storage.LoadData();

        int countAfterReload = controller.GetProjects().Count;

        int taskCountAfterReload = 0;
        List<Project> reloadedProjects = controller.GetProjects();
        if (reloadedProjects.Count > 0)
        {
            string reloadedProjectId = reloadedProjects[0].ProjectId;
            taskCountAfterReload = controller.GetTasksByProjectId(reloadedProjectId).Count;
        }

        List<Project> projects = controller.GetProjects();
        string projectIdToDelete = string.Empty;
        if (projects.Count > 0)
        {
            projectIdToDelete = projects[0].ProjectId;
        }

        string deleteMessage;
        bool deleteSuccess = controller.DeleteProject(projectIdToDelete, out deleteMessage);
        int countAfterDelete = controller.GetProjects().Count;

        reportLines.Add("Create Project -> " + createValid + " | Count: " + countAfterCreate + " | Message: " + createMessage);
        reportLines.Add("Pre-assigned Employees -> " + assignedEmployeeCount);
        reportLines.Add("Update Status -> " + statusUpdated + " | Message: " + statusUpdateMessage);
        reportLines.Add("Create Task + Assign -> " + taskCreated + " | Task Count: " + taskCountAfterCreate + " | Message: " + taskCreateMessage);
        reportLines.Add("Invalid Dates Blocked -> " + (!invalidDateAccepted) + " | Message: " + invalidDateMessage);
        reportLines.Add("Save & Reload -> Before: " + countBeforeReload + " | After: " + countAfterReload + " | Task Count After Reload: " + taskCountAfterReload);
        reportLines.Add("Delete Project -> " + deleteSuccess + " | Count After Delete: " + countAfterDelete + " | Message: " + deleteMessage);
        File.WriteAllLines(reportFilePath, reportLines);
    }
}