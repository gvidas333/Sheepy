using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.CategoryType;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;

    public UsersController(UserManager<ApplicationUser> userManager, IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }
    
    [HttpPut("category-order")]
    public async Task<IActionResult> UpdateCategoryOrder([FromBody] ReorderCategoriesDto reorderDto)
    {
        var userId = GetCurrentUserId();
        var success = await _userService.UpdateCategoryOrderAsync(userId, reorderDto.CategoryOrder);

        if (!success)
        {
            return BadRequest("Failed to update category order");
        }
        
        return NoContent();
    }
    
    private Guid GetCurrentUserId()
    {
        return Guid.Parse(_userManager.GetUserId(User));
    }
}