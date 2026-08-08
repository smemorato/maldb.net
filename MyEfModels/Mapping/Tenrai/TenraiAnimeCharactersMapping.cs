using MyEfModels.Entities;
using Api.Tenrai.Dtos.Anime;

namespace MyEfModels.Mapping.Tenrai;

public static class AnimeCharacterMapping
{
    // Character
    public static Character ToCharacterEntity(TenraiCharacterEntryDto dto, int favorites)
    {
        return new Character
        {
            MalId = dto.Mal_Id,
            Name = dto.Name,
            ImageUrl = dto.Images?.Jpg?.Image_Url,
            Favorites = favorites
        };
    }

    // AnimeCharacter
    public static AnimeCharacter ToAnimeCharacterEntity(
        Anime anime,
        Character character,
        TenraiAnimeCharacterDto dto)
    {
        return new AnimeCharacter
        {
            Anime = anime,
            AnimeId = anime.Id,
            Character = character,
            CharacterId = character.Id,
            Role = dto.Role,
            Favorites = dto.Favorites
        };
    }

    public static void UpdateAnimeCharacterEntity(
        AnimeCharacter entity,
        TenraiAnimeCharacterDto dto)
    {
        entity.Role = dto.Role;
        entity.Favorites = dto.Favorites;
    }

    // Voice Actor

    public static Person ToPersonEntity(TenraiPersonEntryDto dto)
    {
        return new Person
        {
            MalId = dto.Mal_Id,
            Name = dto.Name,
            ImageUrl = dto.Images?.Jpg?.Image_Url,
            Favorites = dto.Favorites
        };
    }
    public static AnimeCharacterVoiceActor ToVoiceActorEntity(
        AnimeCharacter animeCharacter,
        Person person,
        string language)
    {
        return new AnimeCharacterVoiceActor
        {
            AnimeCharacterId = animeCharacter.Id,
            PersonId = person.Id,
            Person = person,
            Language = language
        };
    }
}
