namespace Api.Tenrai.Dtos.Anime;

public class TenraiAnimeEpisodesDto
{
    public int Mal_id { get; set; }
    public required string Url { get; set; }
    public string?  Title {get; set; }
    public string? Title_Japanese { get; set; }
    public string? Title_Romanji { get; set; }
    public int? Duration { get; set; }
    public string? Aired { get; set; }
    public decimal Score { get; set; }
    public bool Filler { get; set; }
    public bool Recap { get; set; }
    public  string? Synopsis { get; set; }
    public int Replies { get; set; }
    public string? Forum_Url { get; set; }

}