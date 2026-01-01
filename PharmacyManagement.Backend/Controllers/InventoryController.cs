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
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryHistoryDTO>>> GetAll()
        {
            var histories = await _inventoryService.GetAllHistoriesAsync();
            return Ok(histories);
        }

        [HttpGet("medicine/{medicineId}")]
        public async Task<ActionResult<IEnumerable<InventoryHistoryDTO>>> GetByMedicine(int medicineId)
        {
            var histories = await _inventoryService.GetHistoriesByMedicineAsync(medicineId);
            return Ok(histories);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<InventoryHistoryDTO>>> GetByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var histories = await _inventoryService.GetHistoriesByDateRangeAsync(startDate, endDate);
            return Ok(histories);
        }

        [HttpPost]
        public async Task<ActionResult<InventoryHistoryDTO>> Create([FromBody] CreateInventoryHistoryDTO createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var history = await _inventoryService.CreateInventoryHistoryAsync(createDto);
            if (history == null)
                return NotFound();
            return CreatedAtAction(nameof(GetAll), history);
        }
    }
}
