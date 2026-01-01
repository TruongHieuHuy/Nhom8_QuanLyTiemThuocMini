using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // API lấy danh sách nhóm thuốc: GET api/MedicineGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineGroup>>> GetMedicineGroups()
        {
            // Lấy tất cả các nhóm thuốc đang hoạt động (IsActive = true nếu có)
            return await _context.MedicineGroups.ToListAsync();
        }
    }
}