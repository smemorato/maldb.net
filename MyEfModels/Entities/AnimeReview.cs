    namespace MyEfModels.Entities;

public class AnimeReview
{
    public int Id { get; set; }
    public int AnimeId { get; set; }
    public int MalId { get; set; }
    public string? Type {get; set; }
    public string? Date { get; set; }
    public string? Review { get; set; }
    public int? Score { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public int? Overall { get; set; }
    public int? Nice { get; set; }
    public int? LoveIt { get; set; }
    public int? Funny { get; set; }
    public int? Confusing { get; set; }
    public int? Informative { get; set; }
    public int? WellWritten { get; set; }
    public int? Creative { get; set; }

    public Anime? Anime { get; set; }


}
