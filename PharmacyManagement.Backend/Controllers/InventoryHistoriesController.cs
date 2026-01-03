using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryHistoriesController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public InventoryHistoriesController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/InventoryHistories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var histories = await _context.InventoryHistories
                .Include(h => h.Medicine)
                .OrderByDescending(h => h.CreatedDate)
                .Select(h => new
                {
                    id = h.Id,
                    date = h.CreatedDate,
                    medicineName = h.Medicine.Name,
                    medicineId = h.MedicineId,
                    transactionType = h.TransactionType,
                    quantity = h.Quantity,
                    stockBefore = h.StockBefore,
                    stockAfter = h.StockAfter,
                    reason = h.Reason,
                    notes = h.Notes
                })
                .ToListAsync();

            return Ok(histories);
        }

        // GET: api/InventoryHistories/medicine/5
        [HttpGet("medicine/{medicineId}")]
        public async Task<IActionResult> GetByMedicine(int medicineId)
        {
            var histories = await _context.InventoryHistories
                .Include(h => h.Medicine)
                .Where(h => h.MedicineId == medicineId)
                .OrderByDescending(h => h.CreatedDate)
                .Select(h => new
                {
                    id = h.Id,
                    date = h.CreatedDate,
                    medicineName = h.Medicine.Name,
                    transactionType = h.TransactionType,
                    quantity = h.Quantity,
                    stockBefore = h.StockBefore,
                    stockAfter = h.StockAfter,
                    reason = h.Reason,
                    notes = h.Notes
                })
                .ToListAsync();

            return Ok(histories);
        }

        // GET: api/InventoryHistories/report?startDate=...&endDate=...&type=Import
        [HttpGet("report")]
        public async Task<IActionResult> GetReport(
            [FromQuery] string startDate, 
            [FromQuery] string endDate,
            [FromQuery] string type = null)
        {
            var query = _context.InventoryHistories
                .Include(h => h.Medicine)
                .AsQueryable();

            if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var start))
            {
                query = query.Where(h => h.CreatedDate.Date >= start.Date);
            }

            if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var end))
            {
                query = query.Where(h => h.CreatedDate.Date <= end.Date);
            }

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(h => h.TransactionType == type);
            }

            var histories = await query
                .OrderByDescending(h => h.CreatedDate)
                .Select(h => new
                {
                    id = h.Id,
                    date = h.CreatedDate,
                    medicineName = h.Medicine.Name,
                    medicineId = h.MedicineId,
                    transactionType = h.TransactionType,
                    quantity = h.Quantity,
                    stockBefore = h.StockBefore,
                    stockAfter = h.StockAfter,
                    reason = h.Reason,
                    notes = h.Notes
                })
                .ToListAsync();

            return Ok(histories);
        }

        // POST: api/InventoryHistories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryHistory history)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            history.CreatedDate = DateTime.Now;
            _context.InventoryHistories.Add(history);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = history.Id }, history);
        }
    }
}
