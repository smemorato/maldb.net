namespace Api.Mal.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTimeOffset Joined_At { get; set; }
    
}
