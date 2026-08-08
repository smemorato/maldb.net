using Api.Tenrai.Dtos.Anime;

namespace Api.Tenrai.Dtos.Response;

public class TenraiAnimeRecommendationResponseDto
{
    public List<TenraiAnimeRecommendationDto> Data { get; set; } = new List<TenraiAnimeRecommendationDto>();
}
