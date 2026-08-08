using Api.Tenrai.Dtos.Anime;

namespace Api.Tenrai.Dtos.Response;

public class TenraiAnimeEpisodesesponseDto
{
    public required TenraiPaginationDto Pagination { get; set; }
    public List<TenraiAnimeEpisodesDto> Data { get; set; } = new List<TenraiAnimeEpisodesDto>();
}
