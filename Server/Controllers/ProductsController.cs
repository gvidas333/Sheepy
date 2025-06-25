using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Product;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProductsController(IProductService productService, UserManager<ApplicationUser> userManager)
    {
        _productService = productService;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductCreateDto productCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var newProductDto = await _productService.AddProductAsync(productCreateDto, userId);

        return CreatedAtAction(nameof(GetProduct), new { id = newProductDto.Id }, newProductDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var userId = GetCurrentUserId();
        var product = await _productService.GetProductByIdAsync(id, userId);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var userId = GetCurrentUserId();
        var products = await _productService.GetProductsForUserAsync(userId);
        return Ok(products);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateDto productUpdateDto)
    {
        if (id != productUpdateDto.Id)
        {
            return BadRequest("Id mismatch between route and body");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var success = await _productService.UpdateProductAsync(productUpdateDto, userId);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var userId = GetCurrentUserId();
        var success = await _productService.DeleteProductAsync(id, userId);

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