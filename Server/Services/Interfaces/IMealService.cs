using server.DTOs.Meal;

namespace Server.Services.Interfaces;

public interface IMealService
{
    Task<MealDto> AddMealAsync(MealCreateDto mealCreateDto, Guid userId);
    Task<MealDto?> GetMealByIdAsync(Guid mealId, Guid userId);
    Task<IEnumerable<MealDto>> GetMealsForUserAsync(Guid userId);
    Task<MealDto?> UpdateMealAsync(MealUpdateDto mealUpdateDto, Guid userId);
    Task<bool> DeleteMealAsync(Guid mealId, Guid userId);
}