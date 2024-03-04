using Application.IManagers;
using Domain.Entities;
using Infrastructure.ErrorResponse;
using Infrastructure.Models;
using Infrastructure.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        public CategoryController(ICategoryManager categoryManager) 
        {
            _categoryManager = categoryManager;
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryModel category)
        {
            if (category == null)
            {
                return BadRequest("Category object is null");
            }

            var result = await _categoryManager.AddCategory(category);

            if (result)
            {
                return Ok("Category created successfully");
            }
            else
            {
                return BadRequest("Failed to create category");
            }
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryManager.DeleteCategory(id);

            if (result)
            {
                return Ok("Category deleted successfully");
            }
            else
            {
                return BadRequest("Something went wrong.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById (int id) 
        {
            var category = _categoryManager.GetCategoryById(id);
            if (category == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Category not found",
                    Code = 404
                });
            }

            var categoryDto = new ResponseCategoryBody
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId
            };

            return Ok(categoryDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<IpCategory>>> GetAllCategories()
        {
            List<IpCategory> categories = _categoryManager.GetAllCategories();
            if (categories == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Category not found",
                    Code = 404
                });
            }

            List<ResponseCategoryBody> categoryDtos = categories.Select(category => new ResponseCategoryBody
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId

            }).ToList();
            return Ok(categoryDtos);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryModel categoryModel)
        {

            var existingCategory = _categoryManager.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Category not found",
                    Code = 404
                });
            }
            try
            {
                if (categoryModel.Name != null)
                {
                    existingCategory.Name = categoryModel.Name;
                }

                _categoryManager.UpdateCategory(existingCategory);

                return Ok("Category is updated successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = ex.Message,
                    Code = 4005
                });
            }
        }

    }
}
