using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace PharmacyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineGroupsController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public MedicineGroupsController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/MedicineGroups - Lấy tất cả danh mục
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineGroup>>> GetMedicineGroups([FromQuery] string search = "")
        {
            try
            {
                var query = _context.MedicineGroups.AsQueryable();

                // Tìm kiếm theo tên danh mục
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(g => g.Name.Contains(search));
                }

                var groups = await query.ToListAsync();
                return Ok(new { message = "Lấy danh mục thành công", data = groups });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi: " + ex.Message });
            }
        }

        // GET: api/MedicineGroups/5 - Lấy thông tin một danh mục
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineGroup>> GetMedicineGroup(int id)
        {
            try
            {
                var medicineGroup = await _context.MedicineGroups.FindAsync(id);
                if (medicineGroup == null)
                    return NotFound(new { message = "Danh mục không tồn tại" });

                return Ok(new { message = "Lấy danh mục thành công", data = medicineGroup });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi: " + ex.Message });
            }
        }

        // POST: api/MedicineGroups - Tạo danh mục mới
        [HttpPost]
        public async Task<ActionResult<MedicineGroup>> CreateMedicineGroup([FromBody] MedicineGroup medicineGroup)
        {
            try
            {
                if (string.IsNullOrEmpty(medicineGroup.Name))
                    return BadRequest(new { message = "Tên danh mục không được để trống" });

                // Kiểm tra xem danh mục đã tồn tại chưa
                var existing = await _context.MedicineGroups
                    .FirstOrDefaultAsync(g => g.Name == medicineGroup.Name);
                if (existing != null)
                    return BadRequest(new { message = "Danh mục này đã tồn tại" });

                _context.MedicineGroups.Add(medicineGroup);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMedicineGroup", new { id = medicineGroup.Id }, 
                    new { message = "Tạo danh mục thành công", data = medicineGroup });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi: " + ex.Message });
            }
        }

        // PUT: api/MedicineGroups/5 - Cập nhật danh mục
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicineGroup(int id, [FromBody] MedicineGroup medicineGroup)
        {
            try
            {
                if (id != medicineGroup.Id)
                    return BadRequest(new { message = "ID không khớp" });

                if (string.IsNullOrEmpty(medicineGroup.Name))
                    return BadRequest(new { message = "Tên danh mục không được để trống" });

                var existing = await _context.MedicineGroups.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Danh mục không tồn tại" });

                // Kiểm tra xem tên đã được sử dụng bởi danh mục khác chưa
                var duplicate = await _context.MedicineGroups
                    .FirstOrDefaultAsync(g => g.Name == medicineGroup.Name && g.Id != id);
                if (duplicate != null)
                    return BadRequest(new { message = "Tên danh mục này đã được sử dụng" });

                existing.Name = medicineGroup.Name;
                existing.Description = medicineGroup.Description;

                _context.MedicineGroups.Update(existing);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật danh mục thành công", data = existing });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi: " + ex.Message });
            }
        }

        // DELETE: api/MedicineGroups/5 - Xóa danh mục
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicineGroup(int id)
        {
            try
            {
                var medicineGroup = await _context.MedicineGroups.FindAsync(id);
                if (medicineGroup == null)
                    return NotFound(new { message = "Danh mục không tồn tại" });

                // Kiểm tra xem có sản phẩm trong danh mục không
                var medicinesInGroup = await _context.Medicines
                    .Where(m => m.MedicineGroupId == id)
                    .CountAsync();

                if (medicinesInGroup > 0)
                    return BadRequest(new { message = $"Không thể xóa danh mục vì có {medicinesInGroup} sản phẩm trong đó" });

                _context.MedicineGroups.Remove(medicineGroup);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa danh mục thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi: " + ex.Message });
            }
        }
    }
}