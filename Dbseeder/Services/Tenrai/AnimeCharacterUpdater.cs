using Api.Tenrai;
using MyEfModels.Data;
using MyEfModels.Entities;
using Microsoft.EntityFrameworkCore;
using MyEfModels.Mapping;
using System.Text.Json;
using Api.Tenrai.Dtos.Anime;
using MyEfModels.Mapping.Tenrai;
using System.Runtime.Intrinsics.X86;
using Dbseeder.Services.Shared;

namespace Dbseeder.Services;


public class AnimeCharacterUpdater: AnimeUpdaterBase<List<TenraiAnimeCharacterDto>>
{
    public AnimeCharacterUpdater(ITenraiApiClient tenrai, MyDbContext db)
        : base(tenrai, db) {}

    public override async Task<List<TenraiAnimeCharacterDto>?> FetchDto(int id)
    {
        var response = await _tenrai.GetAnimeCharactersAsync(id);
        return response?.Data;
    }

    public override async Task ApplyUpdate(int id, List<TenraiAnimeCharacterDto> dtos)
    {
        Console.WriteLine($"Updating Anime {id}");
        var anime = await _db.Animes
                .Include(a => a.AnimeCharacters)
                    .ThenInclude(ac => ac.Character)
                .Include(a => a.AnimeCharacters)
                    .ThenInclude(ac => ac.VoiceActors)
                        .ThenInclude(va => va.Person)
                .FirstOrDefaultAsync(a => a.MalId == id);


        if (anime == null)
        {
            // for now i will ony add anime then doing full anime search anything else will be ignored
            return;
        }

        foreach (var dto in dtos)
        {
            int characterMalId = dto.Character.Mal_Id;

            var character = await _db.Characters
                .FirstOrDefaultAsync(p => p.MalId == characterMalId);

            if (character == null)
            {
                character = AnimeCharacterMapping.ToCharacterEntity(dto.Character, dto.Favorites);

                _db.Characters.Add(character);
            }
            else
            {
                character.Name = dto.Character.Name;
                character.Favorites = dto.Favorites;
            }

            var animeCharacter = anime.AnimeCharacters
                .FirstOrDefault(s =>
                    s.Character.MalId == characterMalId);

            if (animeCharacter == null)
            {
                var entity = AnimeCharacterMapping.ToAnimeCharacterEntity(anime, character, dto);
                animeCharacter = entity;
                anime.AnimeCharacters.Add(entity);
                _db.AnimeCharacters.Add(entity);
            }
            else
            {
                AnimeCharacterMapping.UpdateAnimeCharacterEntity(animeCharacter, dto);
            }

            // Add or update voice actors
            foreach (var vaDto in dto.Voice_Actors)
            {
                if (vaDto.Person == null)
                {
                    continue;
                }

                int personMalId = vaDto.Person.Mal_Id;

                // Get or create Person
                var person = await _db.Persons
                    .FirstOrDefaultAsync(p => p.MalId == personMalId);

                if (person == null)
                {
                    person = AnimeCharacterMapping.ToPersonEntity(vaDto.Person);

                    _db.Persons.Add(person);
                }
                else
                {
                    person.Name = vaDto.Person.Name;
                    person.Favorites = vaDto.Person.Favorites;
                }

                // Check if this voice actor already exists
                var existingVA = animeCharacter.VoiceActors
                    .FirstOrDefault(va =>
                        va.PersonId == person.Id &&
                        va.Language == vaDto.Language);

                if (existingVA == null)
                {
                    var newVA = AnimeCharacterMapping.ToVoiceActorEntity(
                        animeCharacter,
                        person,
                        vaDto.Language
                    );

                    animeCharacter.VoiceActors.Add(newVA);
                    _db.AnimeCharacterVoiceActors.Add(newVA);
                }
                else
                {
                    existingVA.Language = vaDto.Language;
                }
            }

            // Remove missing VoiceActores
            var dtoVAKeys = dto.Voice_Actors
                .Where(va => va.Person != null) 
                .Select(va => (va.Person?.Mal_Id, va.Language))
                .ToHashSet();

            var toRemoveVA = animeCharacter.VoiceActors
                .Where(va => !dtoVAKeys.Contains((va.Person.MalId, va.Language)))
                .ToList();

            foreach (var rem in toRemoveVA)
            {
                animeCharacter.VoiceActors.Remove(rem);
                _db.AnimeCharacterVoiceActors.Remove(rem);
            }
        }

            // Remove missing AnimeCharacters
            var dtoCharacterIds = dtos
                .Select(c => c.Character.Mal_Id)
                .ToHashSet();

            var toRemoveCharacters = anime.AnimeCharacters
                .Where(ac => !dtoCharacterIds.Contains(ac.Character.MalId))
                .ToList();

            foreach (var rem in toRemoveCharacters)
            {
                anime.AnimeCharacters.Remove(rem);
                _db.AnimeCharacters.Remove(rem);
            }
    }



}
