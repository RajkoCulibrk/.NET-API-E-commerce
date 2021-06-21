using ECommerce.Dtos.Category;
using ECommerce.Services.CategoriesService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto categoryDto)
        {
            var result = await _categoriesService.CreateCategory(categoryDto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory (UpdateCategoryDto categoryDto)
        {
            var result = await _categoriesService.UpdateCategory(categoryDto);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteCategory( [FromRoute]int id)
        {
            var response = await _categoriesService.DeleteCategory(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _categoriesService.GetCategories();
            return Ok(response);
        }
    }
}
