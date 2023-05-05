using API.Domain.Interfaces;
using API.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public CategoriesController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet("")]
        public async Task<IActionResult> CategoryList()
        {
            var categories = await _unit.Category.GetAllAsync();

            return Ok(new { categories });
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var category = await _unit.Category.GetByIdAsync(id);

            if (category == null)
            {
                return BadRequest(new { error = "The category was not found!" });
            }

            return Ok(new { category });
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(Category category)
        {
            if (string.IsNullOrEmpty(category.CategoryName))
            {
                return BadRequest(new { error = "Category field can not be empty!" });
            }

            category.CreatedAt = DateTime.Now;
            category.UpdatedAt = DateTime.Now;
            if (!await _unit.Category.CreateAsync(category))
            {
                return BadRequest(new { error = "Getting trouble creating category!" });
            }

            return Ok(new { message = "Category created successfully!" });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(Category category)
        {
            var getCategory = await _unit.Category.GetByIdAsync(category.Id);

            if (getCategory == null)
            {
                return BadRequest(new { error = "The category was not found!" });
            }

            if (string.IsNullOrEmpty(category.CategoryName))
            {
                return BadRequest(new { error = "Category field can not be empty!" });
            }

            getCategory.CategoryName = category.CategoryName;
            getCategory.UpdatedAt = DateTime.Now;
            if (!await _unit.Category.UpdateAsync(getCategory))
            {
                return BadRequest(new { error = "Getting trouble updating category!" });
            }

            return Ok(new { message = "Category updated successfully!" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var category = await _unit.Category.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return BadRequest(new { error = "The category was not found!" });
            }

            if (category.Videos!.Count > 0)
            {
                return BadRequest(new { error = "Delete the videos first under this category!" });
            }

            if (!await _unit.Category.DeleteAsync(category))
            {
                return BadRequest(new { error = "Getting trouble deleting category!" });
            }

            return Ok(new { message = "Category deleted successfully!" });
        }
    }
}
