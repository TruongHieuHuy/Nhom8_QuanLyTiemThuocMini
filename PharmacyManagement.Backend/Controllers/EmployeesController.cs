using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.DTOs;
using PharmacyManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public EmployeesController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Position = e.Position,
                Department = e.Department,
                StartDate = e.StartDate,
                Salary = e.Salary,
                Status = e.Status,
                AvatarUrl = e.AvatarUrl
            }).ToList());
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound(new { message = "Nhân viên không tồn tại" });
            }

            return Ok(new EmployeeDTO
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Position = employee.Position,
                Department = employee.Department,
                StartDate = employee.StartDate,
                Salary = employee.Salary,
                Status = employee.Status,
                AvatarUrl = employee.AvatarUrl
            });
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDTO dto)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound(new { message = "Nhân viên không tồn tại" });
            }

            employee.FullName = dto.FullName ?? employee.FullName;
            employee.Email = dto.Email ?? employee.Email;
            employee.PhoneNumber = dto.PhoneNumber ?? employee.PhoneNumber;
            employee.Position = dto.Position ?? employee.Position;
            employee.Department = dto.Department ?? employee.Department;
            employee.Salary = dto.Salary;
            employee.Status = dto.Status ?? employee.Status;
            employee.AvatarUrl = dto.AvatarUrl ?? employee.AvatarUrl;
            employee.LastModifiedDate = System.DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new EmployeeDTO
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Position = employee.Position,
                Department = employee.Department,
                StartDate = employee.StartDate,
                Salary = employee.Salary,
                Status = employee.Status,
                AvatarUrl = employee.AvatarUrl
            });
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployee([FromBody] CreateEmployeeDTO dto)
        {
            var employee = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Position = dto.Position,
                Department = dto.Department,
                StartDate = dto.StartDate,
                Salary = dto.Salary,
                Status = "Active",
                AvatarUrl = dto.AvatarUrl,
                CreatedDate = System.DateTime.Now,
                LastModifiedDate = System.DateTime.Now
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, new EmployeeDTO
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Position = employee.Position,
                Department = employee.Department,
                StartDate = employee.StartDate,
                Salary = employee.Salary,
                Status = employee.Status,
                AvatarUrl = employee.AvatarUrl
            });
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(new { message = "Nhân viên không tồn tại" });
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
