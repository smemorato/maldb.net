    namespace MyEfModels.Entities;

public class Company
{
    public int Id { get; set; }
    public int MalId {get; set; }
    public required string Name { get; set; }
    public string? Type { get; set; }
    public string? Url { get; set; }

    public ICollection<AnimeCompany> AnimeStudios { get; set; } = new List<AnimeCompany> {};

}
