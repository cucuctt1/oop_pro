using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Builders;

public class ProjectBuilder
{
    private readonly Project _project;

    public ProjectBuilder()
    {
        _project = new Project();
    }

    public ProjectBuilder SetName(string name)
    {
        _project.ProjectName = name;
        return this;
    }

    public ProjectBuilder SetDescription(string description)
    {
        _project.Description = description;
        return this;
    }

    public ProjectBuilder SetLeader(Employee leader)
    {
        _project.Leader = leader;

        if (_project.Employees == null)
        {
            _project.Employees = new List<Employee>();
        }

        if (leader != null)
        {
            bool exists = false;
            for (int i = 0; i < _project.Employees.Count; i++)
            {
                if (_project.Employees[i].Id == leader.Id)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                _project.Employees.Add(leader);
            }
        }

        return this;
    }

    public ProjectBuilder SetEmployees(List<Employee> employees)
    {
        if (_project.Employees == null)
        {
            _project.Employees = new List<Employee>();
        }

        _project.Employees.Clear();

        if (employees != null)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                Employee? employee = employees[i];
                if (employee == null)
                {
                    continue;
                }

                bool exists = false;
                for (int j = 0; j < _project.Employees.Count; j++)
                {
                    if (_project.Employees[j].Id == employee.Id)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    _project.Employees.Add(employee);
                }
            }
        }

        if (_project.Leader != null)
        {
            bool leaderExists = false;
            for (int i = 0; i < _project.Employees.Count; i++)
            {
                if (_project.Employees[i].Id == _project.Leader.Id)
                {
                    leaderExists = true;
                    break;
                }
            }

            if (!leaderExists)
            {
                _project.Employees.Add(_project.Leader);
            }
        }

        return this;
    }

    public ProjectBuilder SetStartDate(DateTime startDate)
    {
        _project.StartDate = startDate;
        return this;
    }

    public ProjectBuilder SetEndDate(DateTime endDate)
    {
        _project.EndDate = endDate;
        return this;
    }

    public ProjectBuilder SetStatus(EnumStatus status)
    {
        _project.Status = status;
        return this;
    }

    public Project Build()
    {
        return _project;
    }
}
