namespace MyEfModels.Entities;

public class UserList
{
    public int Id { get; set; }
    public int AnimeId {get; set; }
    public int UserId { get; set; }
    public required string Status { get; set; }
    public string? StartDate { get; set; }
    public string? FinishDate { get; set; }
    public int? Score { get; set; }
    public int? WatchedEpisodes { get; set; }
    public bool? IsRewatching { get; set;}
    public DateTimeOffset UpdateDate { get; set; }
    public string? Tags { get; set; }

    public Anime Anime {get; set;} = null!;
    public User User {get; set;} = null!;
}
