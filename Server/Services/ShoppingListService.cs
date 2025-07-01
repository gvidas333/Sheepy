using Microsoft.AspNetCore.Identity;
using server.DTOs.ShoppingList;
using Server.Mappers;
using Server.Models;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services;

public class ShoppingListService : IShoppingListService
{
    private readonly IProductRepository _productRepository;
    private readonly IMealRepository _mealRepository;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ShoppingListService(IProductRepository productRepository, IMealRepository mealRepository,
        IShoppingListRepository shoppingListRepository, UserManager<ApplicationUser> userManager)
    {
        _productRepository = productRepository;
        _mealRepository = mealRepository;
        _shoppingListRepository = shoppingListRepository;
        _userManager = userManager;
    }

    public async Task<ShoppingListDto> GenerateAsync(GenerateShoppingListDto dto, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        var aggregatedProducts = await AggregateProducts(dto, userId);
        var mealNames = await GetMealNamesAsync(dto.MealIds, userId);
        var shoppingList = CreateNewShoppingList(dto.ListName, userId, aggregatedProducts, mealNames);

        await _shoppingListRepository.AddAsync(shoppingList);

        return shoppingList.ToDto(user?.CategoryOrder);
    }

    private async Task<Dictionary<Guid, (Product product, double quantity)>> AggregateProducts(GenerateShoppingListDto dto, Guid userId)
    {
        var productDictionary = new Dictionary<Guid, (Product product, double quantity)>();

        await AddProductsFromMealsAsync(productDictionary, dto.MealIds, userId);
        await AddIndividualProductsAsync(productDictionary, dto.ProductIds, userId);

        return productDictionary;
    }

    private async Task AddProductsFromMealsAsync(Dictionary<Guid, (Product, double)> productDictionary,
        List<Guid> mealIds, Guid userId)
    {
        foreach (var mealId in mealIds)
        {
            var meal = await _mealRepository.GetByIdAsync(mealId, userId);
            if (meal?.MealProducts != null)
            {
                foreach (var mealProduct in meal.MealProducts)
                {
                    if (productDictionary.ContainsKey(mealProduct.ProductId))
                    {
                        var existingEntry = productDictionary[mealProduct.ProductId];
                        existingEntry.Item2 += mealProduct.Quantity;
                        productDictionary[mealProduct.ProductId] = existingEntry;
                    }
                    else
                    {
                        productDictionary.Add(mealProduct.ProductId, (mealProduct.Product, mealProduct.Quantity));
                    }
                }
            }
        }
    }

    private async Task AddIndividualProductsAsync(Dictionary<Guid, (Product, double)> productDictionary, 
        List<Guid> productIds, Guid userId)
    {
        foreach (var productId in productIds)
        {
            // we assume a quantity of 1 for individual products 
            const int individualProductQuantity = 1;

            if (productDictionary.ContainsKey(productId))
            {
                var existingEntry = productDictionary[productId];
                existingEntry.Item2 += individualProductQuantity;
                productDictionary[productId] = existingEntry;
            }
            else
            {
                var product = await _productRepository.GetByIdAsync(productId, userId);
                if (product != null)
                {
                    productDictionary.Add(productId, (product, individualProductQuantity));
                }
            }
        }
    }
    
    private async Task<List<string>> GetMealNamesAsync(List<Guid> mealIds, Guid userId)
    {
        var selectedMeals = await _mealRepository.GetByIdsAsync(mealIds, userId);
        return selectedMeals.Select(m => m.Name).ToList();
    }

    private ShoppingList CreateNewShoppingList(string name, Guid userId,
        Dictionary<Guid, (Product product, double quantity)> aggregatedProducts, List<string> mealNames)
    {
        var shoppingList = new ShoppingList
        {
            Name = name,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            MealNames = mealNames,
            ShoppingListItems = new List<ShoppingListItem>()
        };

        foreach (var entry in aggregatedProducts)
        {
            shoppingList.ShoppingListItems.Add(new ShoppingListItem
            {
                ProductId = entry.Key,
                Quantity = entry.Value.quantity,
                IsChecked = false
            });
        }
        return shoppingList;
    }

    public async Task<ShoppingListDto> GetByIdAsync(Guid id, Guid userId)
    {
        var shoppingList = await _shoppingListRepository.GetByIdAsync(id, userId);
        
        return shoppingList?.ToDto();
    }

    public async Task<IEnumerable<ShoppingListDto>> GetAllForUserAsync(Guid userId)
    {
        var shoppingLists = await _shoppingListRepository.GetShoppingListsByUserAsync(userId);

        return shoppingLists.ToDto();
    }

    public async Task<ShoppingListDto?> GetLatestForUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        var shoppingList = await _shoppingListRepository.GetLatestForUserAsync(userId);
        return shoppingList?.ToDto(user?.CategoryOrder);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var listToDelete = await _shoppingListRepository.GetByIdAsync(id, userId);

        if (listToDelete is null)
        {
            return false;
        }

        await _shoppingListRepository.DeleteAsync(listToDelete);

        return true;
    }
}