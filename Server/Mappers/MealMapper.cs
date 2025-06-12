using server.DTOs.Meal;
using Server.Models;

namespace Server.Mappers;

public static class MealMapper
{
    public static MealDto ToDto(this Meal meal)
    {
        return new MealDto
        {
            Id = meal.Id,
            Name = meal.Name,
            Description = meal.Description,
            Products = MapMealProductsToDto(meal.MealProducts)
        };
    }

    private static List<ProductInMealDto> MapMealProductsToDto(ICollection<MealProduct>? mealProducts)
    {
        if (mealProducts == null || !mealProducts.Any())
        {
            return new List<ProductInMealDto>();
        }

        return mealProducts.Select(mp => new ProductInMealDto
        {
            ProductId = mp.ProductId,
            Name = mp.Product?.Name ?? "N/A",
            Quantity = mp.Quantity
        }).ToList();
    }

    public static IEnumerable<MealDto> ToDto(this IEnumerable<Meal> meals)
    {
        return meals.Select(m => m.ToDto()).ToList();
    }

    public static Meal ToEntity(this MealCreateDto mealCreateDto)
    {
        return new Meal
        {
            Name = mealCreateDto.Name,
            Description = mealCreateDto.Description
        };
    }

    public static void ToEntity(this MealUpdateDto mealUpdateDto, Meal mealToUpdate)
    {
        mealToUpdate.Name = mealUpdateDto.Name;
        mealToUpdate.Description = mealUpdateDto.Description;
    }
}