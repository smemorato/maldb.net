using MyEfModels.Entities;
using Api.Mal.Dtos;
using Api.Mal.Dtos.UserAnimeList;



namespace MyEfModels.Mapping;

public static class UserAnimeListMapping
{
    public static UserList ToEntity(this UserAnimeListItemDto dto, Anime anime, User user)
    {
        return new UserList
        {
            AnimeId = anime.Id,
            UserId = user.Id,
            Status = dto.List_Status.Status,
            Score = dto.List_Status.Score,
            StartDate = dto.List_Status.Start_Date,
            FinishDate = dto.List_Status.Finish_Date,
            WatchedEpisodes = dto.List_Status.Num_Watched_Episodes,
            IsRewatching = dto.List_Status.Is_Rewatching,
            UpdateDate = dto.List_Status.Updated_At,
            Tags = string.Join(",",dto.List_Status.Tags)
        };
    }

    public static void UpdateEntity(this UserAnimeListItemDto dto, UserList userListEntry)
    {;
            userListEntry.Status = dto.List_Status.Status;
            userListEntry.Score = dto.List_Status.Score;
            userListEntry.WatchedEpisodes = dto.List_Status.Num_Watched_Episodes;
            userListEntry.IsRewatching = dto.List_Status.Is_Rewatching;
            userListEntry.UpdateDate = dto.List_Status.Updated_At;
            userListEntry.StartDate = dto.List_Status.Start_Date;
            userListEntry.FinishDate = dto.List_Status.Finish_Date;
            userListEntry.Tags = string.Join(",",dto.List_Status.Tags);
    }


}

