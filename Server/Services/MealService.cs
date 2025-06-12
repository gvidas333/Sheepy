using server.DTOs.Meal;
using Server.Mappers;
using Server.Models;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services;

public class MealService : IMealService
{
    private readonly IMealRepository _mealRepository;

    public MealService(IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
    }

    public async Task<MealDto> AddMealAsync(MealCreateDto mealCreateDto, Guid userId)
    {
        var meal = mealCreateDto.ToEntity();
        meal.UserId = userId;
        
        AddProductsToMeal(meal, mealCreateDto.Products);

        await _mealRepository.AddAsync(meal);

        var createdMealWithData = await _mealRepository.GetByIdAsync(meal.Id, userId);

        return createdMealWithData!.ToDto();
    }

    private void AddProductsToMeal(Meal meal, List<MealProductForCreateDto> productForCreateDtos)
    {
        meal.MealProducts = new List<MealProduct>();

        foreach (var productDto in productForCreateDtos)
        {
            var mealProduct = new MealProduct
            {
                ProductId = productDto.ProductId,
                Quantity = productDto.Quantity
            };
            meal.MealProducts.Add(mealProduct);
        }
    }

    public async Task<MealDto?> GetMealByIdAsync(Guid mealId, Guid userId)
    {
        var meal = await _mealRepository.GetByIdAsync(mealId, userId);
        return meal?.ToDto();
    }

    public async Task<IEnumerable<MealDto>> GetMealsForUserAsync(Guid userId)
    {
        var meals = await _mealRepository.GetMealsByUserAsync(userId);
        return meals.ToDto();
    }

    public async Task<MealDto?> UpdateMealAsync(MealUpdateDto mealUpdateDto, Guid userId)
    {
        var existingMeal = await _mealRepository.GetByIdAsync(mealUpdateDto.Id, userId);

        if (existingMeal is null)
        {
            return null;
        }
        
        UpdateMealProperties(existingMeal, mealUpdateDto);
        UpdateMealIngredients(existingMeal, mealUpdateDto.Products);

        await _mealRepository.UpdateAsync(existingMeal);

        return existingMeal.ToDto();
    }

    private void UpdateMealProperties(Meal meal, MealUpdateDto mealUpdateDto)
    {
        meal.Name = mealUpdateDto.Name;
        meal.Description = mealUpdateDto.Description;
    }
    
    private void UpdateMealIngredients(Meal meal, List<MealProductForCreateDto> mealProductForCreateDtos)
    {
        meal.MealProducts.Clear();
        
        foreach (var productDto in mealProductForCreateDtos)
        {
            meal.MealProducts.Add(new MealProduct
            {
                ProductId = productDto.ProductId,
                Quantity = productDto.Quantity
            });
        }
    }

    public async Task<bool> DeleteMealAsync(Guid mealId, Guid userId)
    {
        var mealToDelete = await _mealRepository.GetByIdAsync(mealId, userId);

        if (mealToDelete is null)
        {
            return false;
        }

        await _mealRepository.DeleteAsync(mealToDelete);
        return true;
    }
}