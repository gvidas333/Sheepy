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
public class CategoryTypesController : ControllerBase
{
    private readonly ICategoryTypeService _categoryTypeService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CategoryTypesController(ICategoryTypeService categoryTypeService, UserManager<ApplicationUser> userManager)
    {
        _categoryTypeService = categoryTypeService;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddCategoryType([FromBody] CategoryTypeCreateDto categoryTypeCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var newCategory = await _categoryTypeService.AddCategoryTypeAsync(categoryTypeCreateDto, userId);

        return Ok(newCategory);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryTypeById(Guid id)
    {
        var userId = GetCurrentUserId();
        var category = await _categoryTypeService.GetCategoryTypeByIdAsync(id, userId);

        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategoryTypes()
    {
        var userId = GetCurrentUserId();
        var categories = await _categoryTypeService.GetCategoryTypesForUserAsync(userId);
        return Ok(categories);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoryType(Guid id)
    {
        var userId = GetCurrentUserId();
        var success = await _categoryTypeService.DeleteCategoryTypeAsync(id, userId);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        return Guid.Parse(_userManager.GetUserId(User));
    }
}