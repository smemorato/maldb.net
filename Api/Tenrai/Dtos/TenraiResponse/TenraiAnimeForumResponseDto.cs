using Api.Tenrai.Dtos.Anime;

namespace Api.Tenrai.Dtos.Response;

public class TenraiAnimeForumResponseDto
{
    public required TenraiPaginationDto Pagination { get; set; }
    public List<TenraiAnimeForumTopicDto> Data { get; set; } = new List<TenraiAnimeForumTopicDto>();
}
