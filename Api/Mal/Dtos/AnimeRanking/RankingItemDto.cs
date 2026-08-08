namespace Api.Mal.Dtos.AnimeRanking;

public class RankingItemDto
{
    public required RankingAnimeNodeDto Node { get; set; }
    public required RankingDto Ranking { get; set; }
}
