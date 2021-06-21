using AutoMapper;
using ECommerce.Data;
using ECommerce.Data.Models;
using ECommerce.Dtos;
using ECommerce.Dtos.Category;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services.CategoriesService
{
    public class CategoriesService : ICategoriesService
    {
        public DataContext _context { get; set; }
        private readonly IMapper _mapper;

        public CategoriesService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<APIResponse<GetCategoryDto>> CreateCategory(CreateCategoryDto categoryDto)
        {
            var response = new APIResponse<GetCategoryDto>();
            Category category = new Category
            {
                Name=categoryDto.Name
            };
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            response.Data = new GetCategoryDto {CategoryId=category.CategoryId,Name=category.Name };
            response.Message = "Cattegory created successfully.";
            response.Success = true;
            return (response);

        }

        public async Task<APIResponse<GetCategoryDto>> DeleteCategory(int id)
        {
            APIResponse<GetCategoryDto> response = new APIResponse<GetCategoryDto>();
            var category = await _context.Categories.FirstOrDefaultAsync(c=>c.CategoryId==id);
            if (category == null)
            {
                response.StatusCode = 404;
                response.Message = "Category not found";
                
                response.Success = false;
                return response;
            }

            _context.Remove(category);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetCategoryDto>(category);
            response.Message = "Category deleted successfully";
            response.StatusCode = 200;

            return response;
        }

        public async Task<APIResponse<List<GetCategoryDto>>> GetCategories()
        {
            APIResponse<List<GetCategoryDto>> response = new APIResponse<List<GetCategoryDto>>();
            var categories = await _context.Categories.Select(c=>_mapper.Map<GetCategoryDto>(c)).ToListAsync();
            response.Data = categories;
            response.Success = true;
            response.StatusCode = 200;
            return response;
            
        }

        public async Task<APIResponse<GetCategoryDto>> UpdateCategory(UpdateCategoryDto categoryDto)
        {
            APIResponse<GetCategoryDto> response = new APIResponse<GetCategoryDto>();
            var category = await _context.Categories.Include(c=>c.Products).FirstOrDefaultAsync(c => c.CategoryId == categoryDto.CategoryId);
            if (category == null)
            {
                response.StatusCode = 404;
                response.Message = "Category not found";
                response.Success = false;
                return response;
            }
            _mapper.Map(categoryDto, category);
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetCategoryDto>(category);
            response.StatusCode = 200;
            response.Message = "category updated successfully";
            response.Success = true;

            return response;

        }
    }
}
