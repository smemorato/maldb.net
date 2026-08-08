namespace Api.Mal.Dtos.AnimeRanking;

public class RankingResponseDto
{
    public List<RankingItemDto> Data { get; set; } = new();
    public RankingPagingDto? Paging { get; set; }
}
