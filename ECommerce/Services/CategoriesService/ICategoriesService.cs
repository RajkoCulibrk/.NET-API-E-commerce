using ECommerce.Data.Models;
using ECommerce.Dtos;
using ECommerce.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services.CategoriesService
{
    public interface ICategoriesService
    {
        Task<APIResponse<GetCategoryDto>> CreateCategory(CreateCategoryDto categoryDto);

        Task<APIResponse<GetCategoryDto>> UpdateCategory(UpdateCategoryDto categoryDto);

        Task<APIResponse<GetCategoryDto>> DeleteCategory(int id);
        Task<APIResponse<List<GetCategoryDto>>> GetCategories();

    }
}
