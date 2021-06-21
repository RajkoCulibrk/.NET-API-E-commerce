using AutoMapper;
using ECommerce.Data;
using ECommerce.Data.Models;
using ECommerce.Dtos;
using ECommerce.Dtos.Products;
using ECommerce.Helpers;
using ECommerce.Services.CloudinaryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ECommerce.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        public ProductService(DataContext context, IPhotoService photoService, IMapper mapper)
        {
            _context = context;
            _photoService = photoService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetProductImageDto>> AddProductImage(int productId, IFormFile file)
        {
            ServiceResponse<GetProductImageDto> response = new ServiceResponse<GetProductImageDto>();
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found";
                response.Code = 404;
              
            }
            else
            {
                ProductImage image = new ProductImage();
                var picture = await _photoService.AddPhotoAsync(file);
                image.CloudinaryId = picture.PublicId;
                image.PublicUrl = picture.SecureUrl.AbsoluteUri;
                image.Product = product;
                await _context.ProductImages.AddAsync(image);
                await _context.SaveChangesAsync();


                response.Success = true;
                response.Message = "Image uploaded successfully";
                response.Code = 200;
                response.Data = _mapper.Map<ProductImage, GetProductImageDto>(image);
            }

            return response;
        }

        public async Task<GetProductDto> CreateProduct(CreateProductDto productData)
        {
            var photoResult= await _photoService.AddPhotoAsync(productData.file);
            
            var newProduct = new Product 
            {
                Name= productData.Name,
                Description=productData.Description,
                CloudinaryId= photoResult.PublicId,
                PublicUrl= photoResult.SecureUrl.AbsoluteUri,
                Price=productData.Price,
                CategoryId= productData.CategoryId,
                ShortDescription=productData.ShortDescription,
                New=productData.New,
                Featured=productData.Featured
            };
            var getProductDto = _mapper.Map<GetProductDto>(newProduct);

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return getProductDto;
        }

        public async Task<ServiceResponse<GetProductDto>> DeleteProduct(int id)
        {
            var response = new ServiceResponse<GetProductDto>
            { };
           
               
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                if(product == null)
                {
                    response.Message = "Product with this id does not exist";
                    response.Success = false;
                    response.Code = 404;
                }
                else
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    await _photoService.DeletePhotoAsync(product.CloudinaryId);
                    var productDto = _mapper.Map<GetProductDto>(product);
                    response.Data = productDto;
                }
           
           
            
            return response;
        }

        public async Task<ServiceResponse<GetProductImageDto>> DeleteProductImage(int imageId)
        {
            ServiceResponse<GetProductImageDto> response = new ServiceResponse<GetProductImageDto>();
            var image = await _context.ProductImages.FirstOrDefaultAsync(img => img.ProductImageId == imageId);
            if (image == null)
            {
                response.Code=404;
                response.Message = "Image not found";
                response.Success = false;
            }
            else
            {
             
               await _photoService.DeletePhotoAsync(image.CloudinaryId);
                _context.ProductImages.Remove(image);
               await _context.SaveChangesAsync();

              
                


                response.Code = 200;
                response.Message = "Image deleted successfully";
                response.Success = true;
                response.Data = _mapper.Map<GetProductImageDto>(image);
            }

            return response;
        }

        public async Task<ServiceResponse<PaginationResponse<List<GetProductDto>>>> GetAll(RequestParams requestParams)
        {
            ServiceResponse<PaginationResponse<List<GetProductDto>>> response = new ServiceResponse<PaginationResponse<List<GetProductDto>>>();

             var productsContext =  _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images).AsQueryable();
           
     
            if (requestParams.CategoryId.HasValue)
            {
                productsContext = productsContext.Where(p=>p.CategoryId==requestParams.CategoryId);
            }
            switch (requestParams.SortBy) 
            {
                case "Price":
                    productsContext=productsContext.OrderBy(p => p.Price);
                    break;
                case "Date":
                    productsContext=productsContext.OrderBy(p => p.CreatedAt);
                    break;
                default:
                    productsContext=productsContext.OrderBy(p => p.ProductId);
                    break;
            }

            if (requestParams.Order=="DESC")
            {
                productsContext=productsContext.Reverse();
            }

            productsContext = productsContext
                                .Where(p => EF.Functions
                                .Like(p.Name, $"%{requestParams.Like}%"));

            var products = await productsContext
           
                .Select(x => _mapper.Map<GetProductDto>(x))
                .ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
            var total = productsContext.Count();
            double kurac = total / requestParams.PageSize;
            var pages = Math.Ceiling(((double)total / requestParams.PageSize));
            PaginationResponse<List<GetProductDto>> rajko = new PaginationResponse<List<GetProductDto>>();
            rajko.Pages = (int)pages;
            rajko.Total = total;
            rajko.Data = products.ToList();

            response.Data = rajko;
            response.Code = 200;
            response.Message = kurac.ToString();

            return response;
        }

        public async Task<ServiceResponse<List<GetProductDto>>> GetFeatured()
        {
            ServiceResponse<List<GetProductDto>> response = new ServiceResponse<List<GetProductDto>>();
            var products = await _context.Products
                .Where(p => p.Featured == true)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Select(p=>_mapper.Map<GetProductDto>(p))
                .ToListAsync();
            response.Data = products;
            response.Message = "Here are featured products";
            response.Code = 200;
            response.Success = true;
            return response;

        }

        public async Task<ServiceResponse<List<GetProductDto>>> GetNew()
        {
            ServiceResponse<List<GetProductDto>> response = new ServiceResponse<List<GetProductDto>>();
            var products = await _context.Products
                .Where(p => p.New == true)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Select(p => _mapper.Map<GetProductDto>(p))
                .ToListAsync();
            response.Data = products;
            response.Message = "Here are featured products";
            response.Code = 200;
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<GetProductDto>> GetSingleProduct(int id)
        {
            ServiceResponse<GetProductDto> response = new ServiceResponse<GetProductDto>();
            var product = await _context.Products
                .Include(p=>p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                response.Code = 404;
                response.Message = "product not found";
                response.Success = false;
               
                return response;
            }
            response.Success = true;
            response.Code = 200;
            response.Data = _mapper.Map<GetProductDto>(product);
            response.Message = "Here is your product";
            return response;

        }

        public async Task<ServiceResponse<GetProductDto>> UpdateProduct(UpdateProductDto productDto)
        {
            ServiceResponse<GetProductDto> response = new ServiceResponse<GetProductDto>();
            var product = await _context.Products
                .Include(p=>p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == productDto.ProductId);
            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found";
                response.Code = 404;
            }
            else
            {
                if (productDto.file != null)
                {
                   var picture= await _photoService.AddPhotoAsync(productDto.file);
                   await _photoService.DeletePhotoAsync(product.CloudinaryId);
                   product.PublicUrl = picture.SecureUrl.AbsoluteUri;
                   product.CloudinaryId = picture.PublicId;
                }
                _mapper.Map(productDto, product);
                _context.Update(product);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Data = _mapper.Map<GetProductDto>(product);
                response.Code=200;
                response.Message = "Product updated";
            }
            return response;

        }
    }
}
