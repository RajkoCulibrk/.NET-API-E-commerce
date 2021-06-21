using ECommerce.Data.Models;
using ECommerce.Dtos;
using ECommerce.Dtos.Products;
using ECommerce.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<PaginationResponse<List<GetProductDto>>>> GetAll(RequestParams requestParams);

        Task<GetProductDto> CreateProduct(CreateProductDto productData);

        Task<ServiceResponse<GetProductDto>> DeleteProduct(int id);
        Task<ServiceResponse<GetProductDto>> UpdateProduct(UpdateProductDto productDto);
        Task<ServiceResponse<GetProductImageDto>> AddProductImage(int productId,IFormFile file);
        Task<ServiceResponse<GetProductImageDto>> DeleteProductImage(int imageId);
        Task<ServiceResponse<List<GetProductDto>>> GetFeatured();
        Task<ServiceResponse<List<GetProductDto>>> GetNew();
        Task<ServiceResponse<GetProductDto>> GetSingleProduct(int id);
    }
}
