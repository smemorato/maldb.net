namespace Api.Tenrai.Dtos.Anime;

public class TenraiAnimeRecommendationDto
{
    public required TenraiAnimeEntryDto Entry { get; set; }
    public required string Url { get; set; }
    public int Votes { get; set; }
}