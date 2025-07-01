using Microsoft.AspNetCore.Identity;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> UpdateCategoryOrderAsync(Guid userId, List<string> categoryOrder)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return false;
        }

        user.CategoryOrder = categoryOrder;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}