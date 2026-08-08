namespace MyEfModels.Entities;

public class Person
{
    public int Id { get; set; }
    public required int MalId { get; set; }
    public string? WebSite { get; set; }
    public string? ImageUrl { get; set; }
    public string? Name { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public List<string> AlternateNames { get; set; } = new List<string>();
    public string? Birthday { get; set; }
    public int Favorites { get; set; }
    public string? About { get; set; }

    public ICollection<AnimeStaff> AnimeStaffRoles { get; set; } = new List<AnimeStaff>();
    public ICollection<AnimeCharacterVoiceActor> VoiceActingRoles { get; set; } = new List<AnimeCharacterVoiceActor>();

}
