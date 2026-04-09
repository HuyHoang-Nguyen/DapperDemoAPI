using DapperDemoAPI.Entities;
using DapperDemoAPI.IRepositories;
using DapperDemoAPI.Models;
using DapperDemoAPI.QueryModels;
using Microsoft.AspNetCore.Mvc;

namespace DapperDemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var request = await _departmentRepository.GetAllAsync();
            var result = request.Select(d => new DepartmentModel
            {
                Id = d.Id,
                Name = d.Name
            });
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            var result = new DepartmentModel
            {
                Id = department.Id,
                Name = department.Name
            };
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PostDepartmentModel dpmm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var department = new Department
            {
                Name = dpmm.Name
            };
            var id = await _departmentRepository.CreateAsync(department);
            return Ok(id);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PutDepartmentModel dpmm)
        {
            var existCheck = await _departmentRepository.GetByIdAsync(id);
            if (existCheck == null)
            {
                return NotFound();
            }
            var department = new Department
            {
                Id = id,
                Name = dpmm.Name
            };
            await _departmentRepository.UpdateAsync(department);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentRepository.DeleteAsync(id);
            return Ok();
        }
        [HttpGet("empty")]
        public async Task<IActionResult> GetEmpty()
        {
            var request = await _departmentRepository.GetEmptyDepartmentAsync();
            if (request == null || !request.Any())
            {
                return NotFound();
            }
            var result = request.Select(d => new GetEmptyDepartmentQuery
            {
                Name = d?.Name
            });
            return Ok(result);
        }
    }
}
