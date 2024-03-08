namespace Models;

public class AuthorisedUser
{
    public string Id { get; }

    public string Name { get; }

    public string[] Servers { get; }

    public AuthorisedUser(string id, string name, string[] servers)
    {
        Id = id;
        Name = name;
        Servers = servers;
    }
}
