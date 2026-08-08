using MyEfModels.Entities;
using Api.Tenrai.Dtos.Anime;

namespace MyEfModels.Mapping.Tenrai;

public static class AnimeStaffMapping
{
    public static AnimeStaff ToEntity(Anime anime, Person person, string Position)
    {
        return new AnimeStaff
        {
            Anime = anime,
            AnimeId = anime.Id,
            Person = person,
            PersonId = person.Id,
            Position = Position
        };
    }
}


