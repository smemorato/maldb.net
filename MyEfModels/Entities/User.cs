    namespace MyEfModels.Entities;

public class User
{
    public int Id { get; set; }
    public int MalId {get; set; }
    public required string Username { get; set; }
    public string? location { get; set; }
    public DateTimeOffset JoinDate { get; set; }

    public ICollection<UserList> UserList { get; set; } = new List<UserList> {};

}
