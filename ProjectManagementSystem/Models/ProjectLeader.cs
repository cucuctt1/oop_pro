namespace ProjectManagementSystem.Models;

public class ProjectLeader : Employee
{
    private string _leaderRole;

    public ProjectLeader()
        : base()
    {
        _leaderRole = "Project Leader";
        Role = _leaderRole;
    }

    public ProjectLeader(string id, string name, int age, string email)
        : base(id, name, age, email)
    {
        _leaderRole = "Project Leader";
        Role = _leaderRole;
    }

    public override string Role
    {
        get { return _leaderRole; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _leaderRole = "Project Leader";
            }
            else
            {
                _leaderRole = value;
            }
        }
    }

    public override string GetRole()
    {
        return Role;
    }
}
