using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.ShoppingList;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ShoppingListsController : ControllerBase
{
    private readonly IShoppingListService _shoppingListService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ShoppingListsController(IShoppingListService shoppingListService, UserManager<ApplicationUser> userManager)
    {
        _shoppingListService = shoppingListService;
        _userManager = userManager;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateShoppingList([FromBody] GenerateShoppingListDto generateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var newList = await _shoppingListService.GenerateAsync(generateDto, userId);

        return CreatedAtAction(nameof(GetShoppingList), new { id = newList.Id }, newList);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShoppingList(Guid id)
    {
        var userId = GetCurrentUserId();
        var list = await _shoppingListService.GetByIdAsync(id, userId);

        if (list == null)
        {
            return NotFound();
        }

        return Ok(list);
    }

    [HttpGet]
    public async Task<IActionResult> GetShoppingLists()
    {
        var userId = GetCurrentUserId();
        var lists = await _shoppingListService.GetAllForUserAsync(userId);
        return Ok(lists);
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestShoppingList()
    {
        var userId = GetCurrentUserId();
        var list = await _shoppingListService.GetLatestForUserAsync(userId);

        if (list == null)
        {
            return NotFound("No shopping lists found for this user.");
        }

        return Ok(list);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShoppingList(Guid id)
    {
        var userId = GetCurrentUserId();
        var success = await _shoppingListService.DeleteAsync(id, userId);

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