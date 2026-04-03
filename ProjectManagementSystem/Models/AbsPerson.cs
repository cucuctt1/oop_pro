namespace ProjectManagementSystem.Models;

public abstract class AbsPerson
{
    private string _id;
    private string _name;
    private int _age;
    private string _email;

    protected AbsPerson()
    {
        _id = string.Empty;
        _name = string.Empty;
        _age = 0;
        _email = string.Empty;
    }

    protected AbsPerson(string id, string name, int age, string email)
    {
        _id = id == null ? string.Empty : id;
        _name = name == null ? string.Empty : name;
        _age = age;
        _email = email == null ? string.Empty : email;
    }

    public string Id
    {
        get { return _id; }
        set { _id = value == null ? string.Empty : value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value == null ? string.Empty : value; }
    }

    public int Age
    {
        get { return _age; }
        set { _age = value < 0 ? 0 : value; }
    }

    public string Email
    {
        get { return _email; }
        set { _email = value == null ? string.Empty : value; }
    }

    public abstract string GetRole();
}
