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
    [Route("GetCategoryAll")]
    public async Task<IActionResult> GetCategoryAll()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet]
    [Route("GetCategoryById")]
    public async Task<IActionResult> GetCategoryByID(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    [Route("CreateCategory")]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        var created = await _categoryRepository.AddAsync(category);
        return Ok(created);
    }
    [HttpPut]
    [Route("UpdateCategoryById")]
    public async Task<IActionResult> UpdateCategoryById(int id, Category category)
    {
        

        var exists = await _categoryRepository.GetByIdAsync(id);
        if (exists == null) return NotFound();

        await _categoryRepository.UpdateAsync(category);
        return Ok(category);
    }

    [HttpDelete]
    [Route("DeleteCategoryById")]
    public async Task<IActionResult> DeleteCategoryById(int id)
    {
        var success = await _categoryRepository.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
