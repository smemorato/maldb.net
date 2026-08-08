namespace MyEfModels.Entities;

public class AnimeRelation
{
    public int Id { get; set; }
    public int Anime1Id {get; set; }
    public int Anime2Id { get; set; }
    public required string Type { get; set; }
    public Anime? Anime1 {get; set;}
    public Anime? Anime2 {get; set;}
}
