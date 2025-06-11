using server.DTOs.CategoryType;
using Server.Mappers;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services;

public class CategoryTypeService : ICategoryTypeService
{
    private readonly ICategoryTypeRepository _categoryTypeRepository;

    public CategoryTypeService(ICategoryTypeRepository categoryTypeRepository)
    {
        _categoryTypeRepository = categoryTypeRepository;
    }

    public async Task<CategoryTypeDto> AddCategoryTypeAsync(CategoryTypeCreateDto categoryTypeCreateDto, Guid userId)
    {
        var categoryType = categoryTypeCreateDto.ToEntity();
        categoryType.UserId = userId;

        await _categoryTypeRepository.AddAsync(categoryType);

        return categoryType.ToDto();
    }

    public async Task<CategoryTypeDto?> GetCategoryTypeByIdAsync(Guid categoryTypeId, Guid userId)
    {
        var categoryType = await _categoryTypeRepository.GetByIdAsync(categoryTypeId, userId);
        return categoryType?.ToDto();
    }

    public async Task<IEnumerable<CategoryTypeDto>> GetCategoryTypesForUserAsync(Guid userId)
    {
        var categoryTypes = await _categoryTypeRepository.GetCategoryTypesByUserAsync(userId);
        return categoryTypes.ToDto();
    }

    public async Task<bool> DeleteCategoryTypeAsync(Guid categoryTypeId, Guid userId)
    {
        var categoryTypeToDelete = await _categoryTypeRepository.GetByIdAsync(categoryTypeId, userId);

        if (categoryTypeToDelete is null)
        {
            return false;
        }

        await _categoryTypeRepository.DeleteAsync(categoryTypeToDelete);
        return true;
    }
}