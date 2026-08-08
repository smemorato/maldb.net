namespace Api.Mal.Dtos.UserAnimeList;

public class UserAnimeListItemDto
{
    public required UserAnimeNodeDto Node { get; set; }
    public required UserListStatusDto List_Status { get; set; }
}
