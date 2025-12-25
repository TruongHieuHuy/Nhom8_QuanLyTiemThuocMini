using Microsoft.AspNetCore.Mvc;
using PharmacyManagement.DTOs;
using PharmacyManagement.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmacyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetByCustomer(int customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerAsync(customerId);
            return Ok(orders);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Create([FromBody] CreateOrderDTO createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.CreateOrderAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, status);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
