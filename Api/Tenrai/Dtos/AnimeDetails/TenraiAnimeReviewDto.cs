namespace Api.Tenrai.Dtos.Anime;

public class TenraiAnimeReviewDto
{
    public int Mal_Id { get; set; }
    public required string Url { get; set; }
    public required string Type { get; set; }
    public TenraiAnimeReviewReactionDto? Reactions { get; set; }
    public required string Date { get; set;}
    public required string Review { get; set;}    
    public int? Score { get; set;}
    public List<string> Tags { get; set; } = new List<string>();
    public bool Is_Spoiler { get; set; }
    public bool Is_Premilinary { get; set; }
    public int? Episodes_Watched { get; set; }
    public TenraiUserDto? User { get; set;}

}