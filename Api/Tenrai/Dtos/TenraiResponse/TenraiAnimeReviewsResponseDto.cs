using Api.Tenrai.Dtos.Anime;

namespace Api.Tenrai.Dtos.Response;

public class TenraiAnimeReviewsResponseDto
{
    public required TenraiPaginationDto Pagination { get; set; }
    public List<TenraiAnimeReviewDto> Data { get; set; } = new List<TenraiAnimeReviewDto>();
}
