using Api.Tenrai;
using MyEfModels.Data;


namespace Dbseeder.Services.Shared;

public abstract class AnimeUpdaterBase<TDto> : IAnimeUpdater<TDto>
{
    protected readonly ITenraiApiClient _tenrai;
    protected readonly MyDbContext _db;

    protected AnimeUpdaterBase(ITenraiApiClient tenrai, MyDbContext db)
    {
        _tenrai = tenrai;
        _db = db;
    }

    public abstract Task<TDto?> FetchDto(int id);
    public abstract Task ApplyUpdate(int id, TDto dto);
}
