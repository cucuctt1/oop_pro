namespace ProjectManagementSystem.Models;

public class Employee : AbsPerson
{
    private string _role;

    public Employee()
        : base()
    {
        _role = "Employee";
    }

    public Employee(string id, string name, int age, string email)
        : base(id, name, age, email)
    {
        _role = "Employee";
    }

    public virtual string Role
    {
        get { return _role; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _role = "Employee";
            }
            else
            {
                _role = value;
            }
        }
    }

    public override string GetRole()
    {
        return Role;
    }

    public override string ToString()
    {
        return Name + " - " + GetRole();
    }
}
