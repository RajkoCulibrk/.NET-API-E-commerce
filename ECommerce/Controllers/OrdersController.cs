using ECommerce.Dtos;
using ECommerce.Dtos.Orders;
using ECommerce.Helpers;
using ECommerce.Services.OrdersServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }
        //make an order from all the items in the users cart and empty the cart afterwards
        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> PlaceOrder()
        {
          var userId = User.GetUserId();
          var response= await _ordersService.MakeOrder(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
          
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("kurac")]
        public async Task<IActionResult> CommitOrder(PlaceOrderDto data)
        {
            var response = await _ordersService.CommitOrder(data);
            return Ok(response);
        }

        //allow user to get all his orders
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] RequestParams requestParams)
        {
            
            var userId = User.GetUserId();
            var response = await _ordersService.GetAllOrdersByUserId(userId,  requestParams);
            return Ok(response);
        }

        //allow admin to get all Orders of one user
        [HttpGet]
        [Route("admin/user/{id}")]
        public async Task<IActionResult> GetOrdersByUserId(string id, [FromQuery] RequestParams requestParams)
        {
            var response = await _ordersService.GetAllOrdersByUserId(id, requestParams);
            return Ok(response);
        }

        // allow admin to get all orders that have been placed
        [Authorize(Roles =("Administrator"))]
        [HttpGet]
        [Route("admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] RequestParams requestParams)
        {
            var response = await _ordersService.GetAllOrders(requestParams);
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole("Administrator");
            var response = await _ordersService.GetOrderById(isAdmin, userId, id);
            if (response.StatusCode == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        //allow logged in user to get his ordered items based on the order id
        [HttpGet]
        [Authorize]
        [Route("getOrderItems/{id:int}")]
        public async Task<IActionResult> GetOrderItems(int id)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole("Administrator");
            var response= await _ordersService.GetOrderItems(isAdmin,id, userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [Authorize(Roles = ("Administrator"))]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteOrderById(int id)
        {
            var response = await _ordersService.DeleteOrderById(id);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }

        [Authorize(Roles = ("Administrator"))]
        [HttpPut]
        [Route("confirm/{id:int}")]
        public async Task<IActionResult> ConfirmUnconfirmOrder(int id)
        {
            var response = await _ordersService.ConfirmUnconfirmOrder(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = ("Administrator"))]
        [HttpPut]
        [Route("send/{id:int}")]
        public async Task<IActionResult> MarkUnmarkOrderAsSent(int id)
        {
            var response = await _ordersService.MarkUnmarkOrderAsSent(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
