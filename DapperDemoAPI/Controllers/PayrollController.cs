using DapperDemoAPI.IRepositories;
using DapperDemoAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DapperDemoAPI.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;
        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }
        [HttpPost]
        public async Task<IActionResult> RunPayroll([FromQuery] int month,[FromQuery] int year)
        {
            var result = await _payrollService.InsertPayrollAsync(month, year);
            return Ok(result);
        }
        [HttpGet("report")]
        public async Task<IActionResult> GetReport(int month, int year)
        {
            var result = await _payrollService.GetPayrollReportAsync(month, year);
            return Ok(result);
        }
    }
}
