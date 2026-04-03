using DapperDemoAPI.Entities;
using DapperDemoAPI.Enums.EnumError;
using DapperDemoAPI.IRepositories;
using DapperDemoAPI.Models.Employee;
using DapperDemoAPI.Repositories;
using DapperDemoAPI.Validators.Employees;
using Microsoft.AspNetCore.Mvc;

namespace DapperDemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeController)
        {
            _employeeRepository = employeeController;
        }
        [HttpGet("top")]
        public async Task<IActionResult> GetTop([FromQuery] int n)
        {
            if (n <= 0 || n > 100)
            {
                return BadRequest("Top must be higher than 0 and lower than 100");
            }
            var result = await _employeeRepository.GetTopAsync(n);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeModel emp)
        {
            var errors = await EmployeeValidator.ValidateCreateAsync(emp, _employeeRepository);

            if (errors.Any())
            {
                return BadRequest(new { Error = errors.Select(e => e.ToString()) });
            }
            var id = await _employeeRepository.CreateAsync(emp);
            return Ok(new { Id = id });
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _employeeRepository.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _employeeRepository.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateEmployeeModel empm)
        {
            var errors = EmployeeValidator.ValidateUpdate(empm);
            if (errors.Any())
            {
                return BadRequest(new { Error = errors.Select(e => e.ToString()) });
            }
            var existCheck = await _employeeRepository.GetByIdAsync(id);
            if (existCheck == null)
            {
                return NotFound();
            }
            var result = await _employeeRepository.UpdateAsync(id, empm);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeRepository.DeleteAsync(id);
            return Ok();
        }
        [HttpGet("newhire/by-month")]
        public async Task<IActionResult> NewHire(int year)
        {
            var result = await _employeeRepository.GetNewHireMonthAsync(year);
            return Ok(result);
        }
    }
}
