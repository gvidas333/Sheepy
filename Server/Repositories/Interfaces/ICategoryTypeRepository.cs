using Server.Models;

namespace Server.Repositories.Interfaces;

public interface ICategoryTypeRepository
{
    Task AddAsync(CategoryType categoryType);
    Task<CategoryType?> GetByIdAsync(Guid categoryTypeId, Guid userId);
    Task<IEnumerable<CategoryType>> GetCategoryTypesByUserAsync(Guid userId);
    Task DeleteAsync(CategoryType categoryType);
}