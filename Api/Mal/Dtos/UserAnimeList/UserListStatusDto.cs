namespace Api.Mal.Dtos.UserAnimeList;

public class UserListStatusDto
{
    public required string Status { get; set; }
    public int? Score { get; set; }

    // MAL uses both names depending on the anime
    public int? Num_Watched_Episodes { get; set; }
    public int? Num_Episodes_Watched { get; set; }

    public bool? Is_Rewatching { get; set; }
    public DateTimeOffset Updated_At { get; set; }
    public string? Start_Date { get; set; }
    public string? Finish_Date { get; set; }
    public List<string> Tags { get; set; } = new List<string> ();
}
