using MyEfModels.Entities;
using Api.Mal.Dtos;
using Api.Mal.Dtos.AnimeRanking;



namespace MyEfModels.Mapping;

public static class UserMapping
{
    public static User ToEntity(this UserDto dto)
    {
        return new User
        {
            MalId = dto.Id,
            Username = dto.Name,
            JoinDate = dto.Joined_At
        };
    }

    public static void UpdateEntity(this UserDto dto, User user)
    {

        user.MalId = dto.Id;
        user.Username = dto.Name;
        user.JoinDate = dto.Joined_At;
    }
    


}

