using AZURE_EXAMPLE.Dto;
using AZURE_EXAMPLE.Models;
using AZURE_EXAMPLE.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AZURE_EXAMPLE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IRepositoryBase<Employee> _employeeRepository;
    private readonly IRepositoryBase<Project> _projectRepository;

    public EmployeesController(IRepositoryBase<Employee> employeeRepository, IRepositoryBase<Project> projectRepository)
    {
        _employeeRepository = employeeRepository;
        _projectRepository = projectRepository;
    }

    [HttpGet]
    public ActionResult<IQueryable<Employee>> GetEmployee(string? name, DateTime? dobFrom, DateTime? dobTo)
    {
        var result = _employeeRepository.Get(null);
        if (!string.IsNullOrEmpty(name)) result = result.Where(x => x.EmployeeName.Contains(name));
        if (dobFrom is not null) result = result.Where(x => x.EmployeeDOB >= dobFrom);
        if (dobTo is not null) result = result.Where(x => x.EmployeeDOB <= dobTo);
        return Ok(result);
    }

    [HttpGet("{employeeId:int}")]
    public async Task<ActionResult<Employee?>> GetEmployeeDetails([FromRoute] int employeeId, CancellationToken _)
    {
        var employee = await _employeeRepository.GetById(employeeId, _);
        if (employee is null) return NotFound("Employee not found");
        employee.Projects = employee.ProjectEmployees.Select(x => x.Project);
        return employee;
    }

    [HttpPost]
    public async Task<ActionResult<Employee?>> CreateEmployee(CreateEmployeeRequest request, CancellationToken _)
    {
        //Data validation
        if (request.EmployeeName.Length < 2 || request.EmployeeName.Length > 150)
            return ValidationProblem("Employee Name length must be between 2 and 150");
        if (request.EmployeeDOB.AddYears(16) > DateTime.Now)
            return ValidationProblem("Employee must be over 16 years old.");
        var employee = new Employee
        {
            EmployeeDepartment = request.EmployeeDepartment,
            EmployeeName = request.EmployeeName,
            EmployeeDOB = request.EmployeeDOB
        };
        var res = await _employeeRepository.Create(employee, _);
        return Ok(res);
    }

    [HttpPut("{employeeId:int}")]
    public async Task<ActionResult<Employee?>> UpdateEmployee([FromRoute] int employeeId, CreateEmployeeRequest request,
        CancellationToken _)
    {
        if (request.EmployeeName.Length < 2 || request.EmployeeName.Length > 150)
            return ValidationProblem("Employee Name length must be between 2 and 150");
        if (request.EmployeeDOB.AddYears(16) > DateTime.Now)
            return ValidationProblem("Employee must be over 16 years old.");

        var employee = await _employeeRepository.GetById(employeeId, _);
        if (employee is null) return NotFound("Employee not found.");
        employee.EmployeeDepartment = request.EmployeeDepartment;
        employee.EmployeeName = request.EmployeeName;
        employee.EmployeeDOB = request.EmployeeDOB;
        var res = await _employeeRepository.Update(employee, _);
        return Ok(res);
    }

    [HttpDelete("{employeeId:int}")]
    public async Task<ActionResult<Employee?>> DeleteEmployee([FromRoute] int employeeId, CancellationToken _)
    {
        var res = await _employeeRepository.Delete(employeeId, _);
        return Ok(res);
    }
}