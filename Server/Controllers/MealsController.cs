using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.Meal;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MealsController : ControllerBase
{
    private readonly IMealService _mealService;
    private readonly UserManager<ApplicationUser> _userManager;

    public MealsController(IMealService mealService, UserManager<ApplicationUser> userManager)
    {
        _mealService = mealService;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddMeal([FromBody] MealCreateDto mealCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var newMealDto = await _mealService.AddMealAsync(mealCreateDto, userId);

        return CreatedAtAction(nameof(GetMeal), new { id = newMealDto.Id }, newMealDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMeal(Guid id)
    {
        var userId = GetCurrentUserId();
        var meal = await _mealService.GetMealByIdAsync(id, userId);

        if (meal == null)
        {
            return NotFound();
        }

        return Ok(meal);
    }

    [HttpGet]
    public async Task<IActionResult> GetMeals()
    {
        var userId = GetCurrentUserId();
        var meals = await _mealService.GetMealsForUserAsync(userId);
        return Ok(meals);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeal(Guid id, [FromBody] MealUpdateDto mealUpdateDto)
    {
        if (id != mealUpdateDto.Id)
        {
            return BadRequest("ID mismatch");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var updatedMeal = await _mealService.UpdateMealAsync(mealUpdateDto, userId);

        if (updatedMeal == null)
        {
            return NotFound();
        }

        return Ok(updatedMeal);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeal(Guid id)
    {
        var userId = GetCurrentUserId();
        var success = await _mealService.DeleteMealAsync(id, userId);

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