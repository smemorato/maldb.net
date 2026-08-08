using MyEfModels.Entities;
using Api.Tenrai.Dtos.CharacterDetails;

namespace MyEfModels.Mapping.Tenrai;

public static class TenraiCharacterDetailsMapping
{
    // Character
    public static Character ToEntity(TenraiCharacterDetailsDto dto)
    {
        return new Character
        {
            MalId = dto.Mal_Id,
            Name = dto.Name,
            ImageUrl = dto.Images?.Jpg?.Image_Url,
            NameKanji = dto.Name_Kanji,
            Nicknames = dto.NickNames,
            Favorites = dto.Favorites,
            About = dto.About
        };
    }

    public static void UpdateEntity(Character character, TenraiCharacterDetailsDto dto)
    {

            
            character.Name = dto.Name;
            character.ImageUrl = dto.Images?.Jpg?.Image_Url;
            character.NameKanji = dto.Name_Kanji;
            character.Nicknames = dto.NickNames;
            character.Favorites = dto.Favorites;
            character.About = dto.About;
    
    }


    
}
