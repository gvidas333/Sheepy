using server.DTOs.CategoryType;
using Server.Models;

namespace Server.Services.Interfaces;

public interface ICategoryTypeService
{
    Task<CategoryTypeDto> AddCategoryTypeAsync(CategoryTypeCreateDto categoryTypeCreateDto, Guid userId);
    Task<CategoryTypeDto?> GetCategoryTypeByIdAsync(Guid categoryTypeId, Guid userId);
    Task<IEnumerable<CategoryTypeDto>> GetCategoryTypesForUserAsync(Guid userId);
    Task<bool> DeleteCategoryTypeAsync(Guid categoryTypeId, Guid userId);
}