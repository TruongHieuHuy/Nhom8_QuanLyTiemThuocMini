using Microsoft.AspNetCore.Mvc;
using PharmacyManagement.DTOs;
using PharmacyManagement.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PharmacyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAll()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng" });
            }
            // SỬA: Trả về CustomerDTO thay vì CustomerDetailDTO
            return Ok(customer);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> Create([FromBody] CreateCustomerDTO createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newCustomer = await _customerService.CreateCustomerAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = newCustomer.Id }, newCustomer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        // SỬA: Dùng CreateCustomerDTO cho việc cập nhật (Update) luôn
        public async Task<IActionResult> Update(int id, [FromBody] CreateCustomerDTO updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _customerService.UpdateCustomerAsync(id, updateDto);
            if (!result)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng để cập nhật" });
            }

            return NoContent();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng để xóa" });
            }

            return NoContent();
        }
    }
}