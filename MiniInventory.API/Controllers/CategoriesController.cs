using Microsoft.AspNetCore.Mvc;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        var created = await _categoryRepository.AddAsync(category);
        return Ok(created);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Category category)
    {
        if (id != category.Id)
            return BadRequest("ID mismatch");

        var exists = await _categoryRepository.GetByIdAsync(id);
        if (exists == null) return NotFound();

        await _categoryRepository.UpdateAsync(category);
        return Ok(category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _categoryRepository.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
