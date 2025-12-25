using Microsoft.AspNetCore.Mvc;
using PharmacyManagement.DTOs;
using PharmacyManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmacyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetAll()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDTO>> GetById(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return NotFound();
            return Ok(supplier);
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> Search(string searchTerm)
        {
            var suppliers = await _supplierService.SearchSuppliersAsync(searchTerm);
            return Ok(suppliers);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierDTO>> Create([FromBody] CreateSupplierDTO createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplier = await _supplierService.CreateSupplierAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
        }

        [HttpPut]
        public async Task<ActionResult<SupplierDTO>> Update([FromBody] UpdateSupplierDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplier = await _supplierService.UpdateSupplierAsync(updateDto);
            if (supplier == null)
                return NotFound();
            return Ok(supplier);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _supplierService.DeleteSupplierAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
