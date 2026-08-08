namespace Api.Mal.Dtos.UserAnimeList;

public class UserAnimeListDto
{
    public List<UserAnimeListItemDto> Data { get; set; } = new();
    public PagingDto? Paging { get; set; }
}
