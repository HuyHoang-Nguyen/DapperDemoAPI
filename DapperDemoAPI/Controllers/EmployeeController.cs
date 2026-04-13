using DapperDemoAPI.Models.Employee;
using DapperDemoAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
 

namespace DapperDemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet("top")]
        public async Task<IActionResult> GetTop([FromQuery] int n)
        {
            var result = await _employeeService.GetTopAsync(n);
            return Ok(result);
        }
        //MethodResult

        //[HttpPost]
        //public async Task<IActionResult> Create(EmployeeModel emp)
        //{
        //    var result = await _employeeService.CreateAsync(emp);
        //    return result.GetActionResult();
        //}
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeModel emp)
        {
            var id = await _employeeService.CreateAsync(emp);
            return StatusCode(201, id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _employeeService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _employeeService.GetByIdAsync(id);
            return Ok(result);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, UpdateEmployeeModel emp)
        //{
        //    var result = await _employeeService.UpdateAsync(id, emp);
        //    return result.GetActionResult();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateEmployeeModel emp)
        {
            await _employeeService.UpdateAsync(id, emp);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("newhire/by-month")]
        public async Task<IActionResult> NewHire(int year)
        {
            var result = await _employeeService.GetNewHireMonthAsync(year);
            return Ok(result);
        }
    }
}
