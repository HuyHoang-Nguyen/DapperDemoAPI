using DapperDemoAPI.IRepositories;
using DapperDemoAPI.Repositories;
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
        public async Task<IActionResult> RunPayroll(int month, int year)
        {
            var inserted = await _payrollRepository.InsertPayrollAsync(month, year);
            return Ok(new { RowInserted = inserted});
        }
    }
}
