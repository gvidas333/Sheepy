namespace Server.Services.Interfaces;

public interface IUserService
{
    Task<bool> UpdateCategoryOrderAsync(Guid userId, List<string> categoryOrder);
}