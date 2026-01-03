using Microsoft.AspNetCore.Mvc;
using PharmacyManagement.DTOs;
using PharmacyManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmacyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAll()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<CustomerDetailDTO>> GetDetail(int id)
        {
            var customer = await _customerService.GetCustomerDetailAsync(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> Search(string searchTerm)
        {
            var customers = await _customerService.SearchCustomersAsync(searchTerm);
            return Ok(customers);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> Create([FromBody] CreateCustomerDTO createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _customerService.CreateCustomerAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPut]
        public async Task<ActionResult<CustomerDTO>> Update([FromBody] UpdateCustomerDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _customerService.UpdateCustomerAsync(updateDto);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
