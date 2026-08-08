    namespace MyEfModels.Entities;

public class AnimeCompany
{
    public int Id { get; set; }
    public int AnimeId {get; set; }
    public int CompanyId { get; set; }
    public required string Role { get; set; }
    public Company? Company { get; set; }
    public Anime? Anime { get; set; }

}
