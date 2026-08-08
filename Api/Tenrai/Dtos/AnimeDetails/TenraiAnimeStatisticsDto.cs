namespace Api.Tenrai.Dtos.Anime;

public class TenraiAnimeStatisticsDto
{
    public int Watching { get; set; }
    public int Completed { get; set; }
    public int On_Hold { get; set; }
    public int Dropped { get; set; }
    public int Plan_To_Watch { get; set; }
    public int Total { get; set; }
    public List<TenraiScoreStatsDto> Scores { get; set; } = new List<TenraiScoreStatsDto>();
    
}