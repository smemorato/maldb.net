using Api.Tenrai.Dtos.Anime;

namespace Api.Tenrai.Dtos.Response;

public class TenraiAnimeEpisodesResponseDto
{
    public required TenraiPaginationDto Pagination { get; set; }
    public List<TenraiAnimeEpisodeDto> Data { get; set; } = new List<TenraiAnimeEpisodeDto>();
}
