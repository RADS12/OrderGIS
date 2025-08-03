using Microsoft.AspNetCore.Mvc;
using OrderGIS.Enums;
using OrderGIS.Interfaces;
using OrderGIS.Models;

namespace OrderGIS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        //CSRF - Cross-Site Request Forgery
        //CORS - Cross-Origin Resource Sharing.Sever will return Access-Control-Allow-Origin

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderId = _orderService.CreateOrder(order);

            if (orderId == null) 
                return StatusCode(StatusCodes.Status500InternalServerError, "Order not create");

            return Ok(orderId);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult> Order(int orderId)
        {
            if (!ModelState.IsValid || orderId < 0) 
                return BadRequest(ModelState);

            var id = await _orderService.GetOrder(orderId);
            return Ok(id);
        }

        // POST: OrderStatusEnum/Edit/5
        [HttpPost("{orderId}/status")]
        public async Task<ActionResult> UpdateStatus(int orderId, [FromBody] OrderStatus status)
        {
            if (orderId != null && string.IsNullOrEmpty(status.ToString()))
                return BadRequest();

            await _orderService.UpdateOrder(orderId, status);
            return Ok();
        }

        [HttpPost("{orderId}")]
        public async Task<ActionResult> Delete(int orderId, [FromBody] OrderStatus status)
        {
            if (orderId != null)
                return BadRequest();

            await _orderService.DeleteOrder(orderId);
            return Ok();
        }
    }
}
