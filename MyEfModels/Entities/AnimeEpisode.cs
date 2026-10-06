    namespace MyEfModels.Entities;

public class AnimeEpisode
{
    public int Id { get; set; }
    public int AnimeId { get; set; }
    public int EpisodeNumber { get; set; }
    public string? Url { get; set; }
    public string? Title { get; set; }
    public string? TitleRomanji {get; set; }
    public string? TitleJapanese { get; set; }
    public int? Duration { get; set; }
    public string? Aired { get; set; }
    public decimal? Score { get; set; }
    public bool Filler { get; set; }
    public bool Recap { get; set; }
    public string? Synopsis { get; set; }
    public int? Replies { get; set; }
    public string? ForumUrl { get; set; }

    public Anime? Anime { get; set; }


}
