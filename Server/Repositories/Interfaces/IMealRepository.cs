using Server.Models;

namespace Server.Repositories.Interfaces;

public interface IMealRepository
{
    Task AddAsync(Meal meal);
    Task<Meal?> GetByIdAsync(Guid mealId, Guid userId);
    Task<IEnumerable<Meal>> GetMealsByUserAsync(Guid userId);
    Task<IEnumerable<Meal>> GetByIdsAsync(List<Guid> mealIds, Guid userId);
    Task UpdateAsync(Meal meal);
    Task DeleteAsync(Meal meal);
}