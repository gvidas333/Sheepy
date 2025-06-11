using server.DTOs.CategoryType;
using Server.Models;

namespace Server.Mappers;

public static class CategoryTypeMapper
{
    public static CategoryTypeDto ToDto(this CategoryType categoryType)
    {
        return new CategoryTypeDto
        {
            Id = categoryType.Id,
            Name = categoryType.Name
        };
    }

    public static IEnumerable<CategoryTypeDto> ToDto(this IEnumerable<CategoryType> categoryTypes)
    {
        return categoryTypes.Select(ct => ct.ToDto());
    }

    public static CategoryType ToEntity(this CategoryTypeCreateDto categoryTypeCreateDto)
    {
        return new CategoryType
        {
            Name = categoryTypeCreateDto.Name
        };
    }
}
