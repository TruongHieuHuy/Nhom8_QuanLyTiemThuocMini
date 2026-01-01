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
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicinesController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineDTO>>> GetAll()
        {
            var medicines = await _medicineService.GetAllMedicinesAsync();
            return Ok(medicines);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineDTO>> GetById(int id)
        {
            var medicine = await _medicineService.GetMedicineByIdAsync(id);
            if (medicine == null)
                return NotFound();
            return Ok(medicine);
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<MedicineDTO>>> Search(string searchTerm)
        {
            var medicines = await _medicineService.SearchMedicinesAsync(searchTerm);
            return Ok(medicines);
        }

        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<IEnumerable<MedicineDTO>>> GetByGroup(int groupId)
        {
            var medicines = await _medicineService.GetMedicinesByGroupAsync(groupId);
            return Ok(medicines);
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<MedicineDTO>>> GetLowStock()
        {
            var medicines = await _medicineService.GetLowStockMedicinesAsync();
            return Ok(medicines);
        }

        [HttpGet("expired")]
        public async Task<ActionResult<IEnumerable<MedicineDTO>>> GetExpired()
        {
            var medicines = await _medicineService.GetExpiredMedicinesAsync();
            return Ok(medicines);
        }

        [HttpPost]
        public async Task<ActionResult<MedicineDTO>> Create([FromBody] CreateMedicineDTO createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medicine = await _medicineService.CreateMedicineAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = medicine.Id }, medicine);
        }

        [HttpPut]
        public async Task<ActionResult<MedicineDTO>> Update([FromBody] UpdateMedicineDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medicine = await _medicineService.UpdateMedicineAsync(updateDto);
            if (medicine == null)
                return NotFound();
            return Ok(medicine);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _medicineService.DeleteMedicineAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
