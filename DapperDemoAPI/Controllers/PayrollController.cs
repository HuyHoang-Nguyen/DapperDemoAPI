using DapperDemoAPI.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperDemoAPI.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollRepository _payrollRepository;
        public PayrollController(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }
        [HttpPost]
        public async Task<IActionResult> RunPayroll([FromQuery] int month,[FromQuery] int year)
        {
            var result = await _payrollRepository.InsertPayrollAsync(month, year);
            return Ok(result);
        }
        [HttpGet("report")]
        public async Task<IActionResult> GetReport([FromQuery] int month,[FromQuery] int year)
        {
            var result = await _payrollRepository.GetPayrollReportAsync(month, year);
            return Ok(result);
        }
    }
}
