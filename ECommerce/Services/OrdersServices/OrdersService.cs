using AutoMapper;
using ECommerce.Data;
using ECommerce.Data.Models;
using ECommerce.Dtos;
using ECommerce.Dtos.Orders;
using ECommerce.Dtos.Products;
using ECommerce.Dtos.ViewDtos;
using ECommerce.Services.EMailService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ECommerce.Services.OrdersServices
{
    public class OrdersService : IOrdersService
    {
        private readonly DataContext _context;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;


        public OrdersService(DataContext context, IEmailService emailService, IMapper mapper)
        {
            _context = context;
            _emailService = emailService;
            _mapper = mapper;
        }

        /// get all orders from the db for admin
        public async Task<APIResponse<PaginationResponse<List<GetOrderDto>>>> GetAllOrders(RequestParams requestParams)
        {
            APIResponse<PaginationResponse<List<GetOrderDto>>> response = new APIResponse<PaginationResponse<List<GetOrderDto>>>();
            var orders =  _context.Orders
                .Select(o => _mapper.Map<GetOrderDto>(o));

            var total = orders.Count();
            var ordersPaged=await orders.ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
            var pages = Math.Ceiling(((double)total / requestParams.PageSize));


            PaginationResponse<List<GetOrderDto>> pagination = new PaginationResponse<List<GetOrderDto>>();
            pagination.Data = ordersPaged.ToList();
            pagination.Pages = (int)pages;
            pagination.Total = total;
            response.Data = pagination;
            return response;
        }

        //get all orders of one specific user
        public async Task<APIResponse<PaginationResponse<List<GetOrderDto>>>> GetAllOrdersByUserId(string userId, RequestParams requestParams)
        {
            APIResponse<PaginationResponse<List<GetOrderDto>>> response = new APIResponse<PaginationResponse<List<GetOrderDto>>>();

            var orders =  _context.Orders.
                Where(o => o.UserId == userId).
                Select(o => _mapper.Map<GetOrderDto>(o));

            var total = orders.Count();
            var ordersPaged = await orders.ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
            var pages = Math.Ceiling(((double)total / requestParams.PageSize));


            PaginationResponse<List<GetOrderDto>> pagination = new PaginationResponse<List<GetOrderDto>>();
            pagination.Data = ordersPaged.ToList();
            pagination.Pages = (int)pages;
            pagination.Total = total;
            response.Data = pagination;

            return response;
        }

        public async Task<APIResponse<List<GetOrderItemDto>>> GetOrderItems(bool isAdmin,int orderId, string userId)
        {
            APIResponse<List<GetOrderItemDto>> response = new APIResponse<List<GetOrderItemDto>>();
            if (isAdmin)
            {
                var order = await _context.Orders
                                             .Include(o => o.OrderItems)
                                             .ThenInclude(oi => oi.Product)
                                             .FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (order == null)
                {
                    response.Message = "Order was not found";
                    response.StatusCode = 404;
                    response.Success = false;
                    return response;
                }

                var orderItems = order.OrderItems;
                var orderItemsDtos = orderItems
                     .Select(oi => _mapper.Map<GetOrderItemDto>(oi))
                     .ToList();

                response.Data = orderItemsDtos;
            }
            else
            {
                var order = await _context.Orders
                                            .Include(o => o.OrderItems)
                                            .ThenInclude(oi => oi.Product)
                                            .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);
                if (order == null)
                {
                    response.Message = "Order was not found";
                    response.StatusCode = 404;
                    response.Success = false;
                    return response;
                }

                var orderItems = order.OrderItems;
                var orderItemsDtos = orderItems
                     .Select(oi => _mapper.Map<GetOrderItemDto>(oi))
                     .ToList();

                response.Data = orderItemsDtos;
            }
            return response;
        }
      
        public async Task<APIResponse<bool>> MakeOrder(string userId)
        {
            ApiUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            List<CartItem> cartItems = await _context.Cart
                .Where(o => o.UserId == userId)
                .Include(ci => ci.Product)
                .ToListAsync();
            APIResponse<bool> response = new APIResponse<bool>();
            var order = new Order
            {
                UserId = userId,
                Street=user.Street,
                City=user.City,
                Email=user.Email,
                Phone=user.PhoneNumber,
                HouseNumber=user.HouseNumber,
                Country=user.Country,
                FirstName=user.FirstName,
                LastName=user.LastName

            };
            if (cartItems.Count() < 1)
            {
                response.Message = "There were no items in the cart to order.";
                response.StatusCode = 404;
                response.Success = false;
                return response;
            }
            await _context.Orders.AddAsync(order);
            List<OrderItemInvoiceDto> invoiceItems = new List<OrderItemInvoiceDto>();
            foreach (CartItem cartItem in cartItems)
            {
                invoiceItems.Add(
                    new OrderItemInvoiceDto
                    {
                        Ammount = cartItem.Ammount,
                        Price = cartItem.Product.Price,
                        Name = cartItem.Product.Name,
                        Picture = cartItem.Product.PublicUrl
                    }
                    );
                await _context.OrderItems.AddAsync(
                     new OrderItem
                     {
                         Order = order,
                         ProductId = cartItem.ProductId,
                         Ammount = cartItem.Ammount,
                         Price = cartItem.Product.Price
                     }
                     ); ;
            }
            _context.Cart.RemoveRange(cartItems);
            var result = await _context.SaveChangesAsync();
            var model = _mapper.Map<InvoiceViewModel>(order);

            model.Title = "Order placed successfuly";
            model.Message = "You have successfully placed your order. Down you will find  the detailed  list of items that you ordered.";


            _emailService.SendHtmlGmail(order.User.Email, $"Order #{order.OrderId}", order.User.FirstName, 4, model);

            response.Success = Convert.ToBoolean(result);




            return response;


        }
        public async Task<APIResponse<bool>> DeleteOrderById(int orderId)
        {
            APIResponse<bool> response = new APIResponse<bool>();
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Order with specified id was not found";
                response.StatusCode = 404;
                return response;
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            response.Message = "Order deleted successfullty";
            response.StatusCode = 200;
            return response;



        }

        public async Task<APIResponse<bool>> MarkUnmarkOrderAsSent(int orderId)
        {
            APIResponse<bool> response = new APIResponse<bool>();
            var order = await _context.Orders.Include(o=>o.User).FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                response.Message = "Order not found";
                response.Success = false;
                response.StatusCode = 404;
                return response;
            }
            order.Sent = !order.Sent;
            _context.Orders.Update(order);
            var success = await _context.SaveChangesAsync();
            if (success > 0)
            {
                if (order.Sent)
                {
                    var model = _mapper.Map<ConfirmSentModel>(order);
                    model.Title = "Order Shipped";
                    model.Message = "Your order has been shipped.";
                    if(order.User != null)
                    {
                        _emailService.SendHtmlGmail(order.User.Email, order.User.FirstName, $"Order #{order.OrderId} Sent", 3, model);
                    }
                    else
                    {
                        _emailService.SendHtmlGmail(order.Email, order.FirstName, $"Order #{order.OrderId} Sent", 3, model);
                    }
                    response.Data = true;
                };
                response.Message = "Change made successfully";
                response.Success = true;
              
                response.StatusCode = 200;
                
            }
            return response;
        }
        public async Task<APIResponse<bool>> ConfirmUnconfirmOrder(int orderId)
        {
            APIResponse<bool> response = new APIResponse<bool>();
            var order = await _context.Orders
                .Include(o=>o.User)
                .Include(o=>o.OrderItems)
                .ThenInclude(oi=>oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                response.Message = "Order not found";
                response.Success = false;
                response.StatusCode = 404;
                return response;
            }
            order.Confirmed = !order.Confirmed;
            _context.Orders.Update(order);
            var success = await _context.SaveChangesAsync();
            if (success > 0)
            {
                if (order.Confirmed)
                {
                    var model = _mapper.Map<InvoiceViewModel>(order);
                    response.Data = true;
                    model.Title = "Order confirmed";
                    model.Message = "Your order has been confirmed you will be informed when we ship the order. Down you will find the invoice with the detailed list of items that you ordered.";

                    if (order.User != null)
                    {
                        _emailService.SendHtmlGmail(order.User.Email, "Order confirmed", order.User.FirstName, 4, model);
                    }
                    else
                    {
                        _emailService.SendHtmlGmail(order.Email, "Order confirmed", order.FirstName, 4, model);
                    }
                   
                }
                response.Message = "Change made successfully";
            }
            return response;
        }

        public async Task<APIResponse<GetOrderDto>> GetOrderById(bool isAdmin, string userId, int orderId)
        {
            APIResponse<GetOrderDto> response = new APIResponse<GetOrderDto>();
            if (!isAdmin)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);
                if (order == null)
                {
                    response.Success = false;
                    response.StatusCode = 404;
                    response.Message = "Order with the specified id was not found";
                    return response;
                }
                response.Data = _mapper.Map<GetOrderDto>(order);
                return response;
            }
            else
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (order == null)
                {
                    response.Success = false;
                    response.StatusCode = 404;
                    response.Message = "Order with the specified id was not found";
                    return response;
                }
                response.Data = _mapper.Map<GetOrderDto>(order);
                return response;
            }

        }

        public async Task<APIResponse<bool>> CommitOrder(PlaceOrderDto data)
        {
            APIResponse<bool> response = new APIResponse<bool>();
            var order = new Order();
            order.City = data.City;
            order.Country = data.Country;
            order.Email = data.Email;
            order.HouseNumber = data.HouseNumber;
            order.Phone = data.Phone;
            order.Street = data.Street;
            order.FirstName = data.FirstName;
            order.LastName = data.LastName;
            

            await _context.AddAsync(order);
            //await _context.SaveChangesAsync();
            foreach (OrderItemAmmountProductId productIdAmmount in data.Products)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductId == productIdAmmount.ProductId);
                OrderItem orderItem = new OrderItem
                {
                    Ammount = productIdAmmount.Ammount,
                    Price = product.Price,
                    Order=order,
                    ProductId=product.ProductId
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
           await _context.SaveChangesAsync();
            var model = _mapper.Map<InvoiceViewModel>(order);

            model.Title = "Order placed successfuly";
            model.Message = "You have successfully placed your order. Down you will find  the detailed  list of items that you ordered.";
            

            _emailService.SendHtmlGmail(order.Email, $"Order #{order.OrderId}", order.FirstName, 4, model);

            return response;

        }
    }
}
