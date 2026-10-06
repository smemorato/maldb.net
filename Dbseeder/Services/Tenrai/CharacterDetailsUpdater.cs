using Api.Tenrai;
using MyEfModels.Data;
using MyEfModels.Entities;
using Microsoft.EntityFrameworkCore;
using MyEfModels.Mapping;
using System.Text.Json;
using Api.Tenrai.Dtos.Anime;
using MyEfModels.Mapping.Tenrai;
using Api.Tenrai.Dtos.Response;
using Dbseeder.Services.Shared;



namespace Dbseeder.Services.Updaters;

public class CharacterDetailsUpdater : AnimeUpdaterBase<TenraiCharacterDetailsResponseDto>
{
    public CharacterDetailsUpdater(ITenraiApiClient tenrai, MyDbContext db)
        : base(tenrai, db) {}

    public override async Task<TenraiCharacterDetailsResponseDto?> FetchDto(int id)
    {
        // Fetch first page
        var response = await _tenrai.GetCharacterDetails(id);
        return response;
    }
    public override async Task ApplyUpdate(int id, TenraiCharacterDetailsResponseDto dto)
    {


        var characterDto= dto.Data;

        var character = await _db.Characters
                .FirstOrDefaultAsync(a => a.MalId == id);

        if (character == null)
        {
            // for now i will ony add anime then doing full anime search anything else will be ignored
            return;
        }


        if (character == null)
        {
            character = TenraiCharacterDetailsMapping.ToEntity(characterDto);
            _db.Characters.Add(character);
        }
        else
        {
            TenraiCharacterDetailsMapping.UpdateEntity(character, characterDto);
        }

        // for now i will not add anime relations


        
        await _db.SaveChangesAsync();

    }

}
