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
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("revenue/date/{date}")]
        public async Task<ActionResult<decimal>> GetRevenueByDate(DateTime date)
        {
            var revenue = await _reportService.GetRevenueByDateAsync(date);
            return Ok(new { revenue });
        }

        [HttpGet("revenue/range")]
        public async Task<ActionResult<decimal>> GetRevenueByRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var revenue = await _reportService.GetRevenueByDateRangeAsync(startDate, endDate);
            return Ok(new { revenue });
        }

        [HttpGet("top-medicines/{count}")]
        public async Task<ActionResult<IEnumerable<MedicineDTO>>> GetTopMedicines(int count = 10)
        {
            var medicines = await _reportService.GetTopSellingMedicinesAsync(count);
            return Ok(medicines);
        }

        [HttpGet("top-customers/{count}")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetTopCustomers(int count = 10)
        {
            var customers = await _reportService.GetTopCustomersAsync(count);
            return Ok(customers);
        }

        [HttpGet("out-of-stock")]
        public async Task<ActionResult<IEnumerable<MedicineDTO>>> GetOutOfStock()
        {
            var medicines = await _reportService.GetOutOfStockMedicinesAsync();
            return Ok(medicines);
        }

        [HttpGet("orders-count/{date}")]
        public async Task<ActionResult<int>> GetOrdersCount(DateTime date)
        {
            var count = await _reportService.GetTotalOrdersByDateAsync(date);
            return Ok(new { count });
        }
    }
}
