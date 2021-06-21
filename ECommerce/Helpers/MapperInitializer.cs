using AutoMapper;
using ECommerce.Data.Models;
using ECommerce.Dtos.Cart;
using ECommerce.Dtos.Category;
using ECommerce.Dtos.Orders;
using ECommerce.Dtos.Products;
using ECommerce.Dtos.Users;
using ECommerce.Dtos.ViewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Helpers
{
    public class MapperInitializer:Profile
    {
        public MapperInitializer()
        {
            CreateMap<ApiUser, CreateUserDto>().ForMember(m=>m.Email,m=>m.MapFrom(u=>u.UserName)).ReverseMap();
            CreateMap<ApiUser, GetUserDto>();
            CreateMap<UpdateUserDto, ApiUser>();

            CreateMap<CartItem, CartItemDto>()
                .ForMember(ciDto => ciDto.Product, y => y.MapFrom(ci => ci.Product))
                .ForMember(ciDto=>ciDto.Ammount,y=>y.MapFrom(ci=>ci.Ammount));

            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, GetProductDto>().ForMember(gp=>gp.Category,m=>m.MapFrom(p=>p.Category));
            CreateMap<ProductImage, GetProductImageDto>();

            CreateMap<OrderItem, GetOrderItemDto>();
            CreateMap<Order, ConfirmSentModel>()
              .ForMember(csm => csm.HouseNumber, m => m.MapFrom(o => o.User.HouseNumber))
              .ForMember(csm => csm.Street, m => m.MapFrom(o => o.User.Street))
              .ForMember(csm => csm.City, m => m.MapFrom(o => o.User.City));
            CreateMap<Order, GetOrderDto>();
            CreateMap<Order, InvoiceViewModel>()
                .ForMember(ivm => ivm.Order, m => m.MapFrom(o => o))
                .ForMember(ivm => ivm.OrderItems, m => m.MapFrom(o => o.OrderItems));

            CreateMap<Category, GetCategoryDto>();
            CreateMap<UpdateCategoryDto, Category>();


        }
    }
}
