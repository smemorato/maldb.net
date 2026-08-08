
namespace Dbseeder.Services.Shared;
public interface IAnimeUpdater<TDto>
{
    Task<TDto?> FetchDto(int id);
    Task ApplyUpdate(int id, TDto dto);
}
