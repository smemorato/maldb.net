using MyEfModels.Entities;
using Api.Tenrai.Dtos.PersonDetails;

namespace MyEfModels.Mapping.Tenrai;

public static class TenraiPersonDetailsMapping
{
    // Character
    public static Person ToEntity(TenraiPersonDetailsDto dto)
    {
        return new Person
        {
            MalId = dto.Mal_Id,
            Name = dto.Name,
            WebSite = dto.Website_Url,
            ImageUrl = dto.Images?.Jpg?.Image_Url,
            GivenName = dto.Given_Name,
            FamilyName = dto.Family_Name,
            AlternateNames = dto.Alternate_Names,
            Birthday = dto.Birthday,
            Favorites = dto.Favorites,
            About = dto.About
        };
    }

    public static void UpdateEntity(Person person, TenraiPersonDetailsDto dto)
    {

        person.Name = dto.Name;
        person.WebSite = dto.Website_Url;
        person.ImageUrl = dto.Images?.Jpg?.Image_Url;
        person.GivenName = dto.Given_Name;
        person.FamilyName = dto.Family_Name;
        person.AlternateNames = dto.Alternate_Names;
        person.Birthday = dto.Birthday;
        person.Favorites = dto.Favorites;
        person.About = dto.About;
    
    }


    public static AnimeStaff ToAnimeStaffEntity (Person person, Anime anime, string position)
    {
        return new AnimeStaff
        {
            AnimeId = anime.Id,
            Anime = anime,
            PersonId = person.Id,
            Person = person,
            Position = position
        };
    }


    
}
