using ECommerce.Data.Models;
using ECommerce.Dtos;
using ECommerce.Dtos.Orders;
using ECommerce.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services.OrdersServices
{
    public interface IOrdersService
    {
        Task<APIResponse<bool>>MakeOrder(string userId);

        Task<APIResponse<bool>> CommitOrder(PlaceOrderDto data);



        Task<APIResponse<GetOrderDto>> GetOrderById(bool isAdmin,string userId,int orderId);


        Task<APIResponse<PaginationResponse<List<GetOrderDto>>>> GetAllOrders(RequestParams requestParams);
        Task<APIResponse<PaginationResponse<List<GetOrderDto>>>> GetAllOrdersByUserId(string userId, RequestParams requestParams);
        Task<APIResponse<Boolean>> ConfirmUnconfirmOrder(int orderId);

        Task<APIResponse<Boolean>> DeleteOrderById(int orderId);

        Task<APIResponse<Boolean>> MarkUnmarkOrderAsSent(int orderId);
        Task<APIResponse<List<GetOrderItemDto>>> GetOrderItems(bool isAdmin,int orderId,string userId);

   
    }
}
