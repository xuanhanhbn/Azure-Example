using AZURE_EXAMPLE.Dto;
using AZURE_EXAMPLE.Models;
using AZURE_EXAMPLE.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AZURE_EXAMPLE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IRepositoryBase<Employee> _employeeRepository;
    private readonly IRepositoryBase<Project> _projectRepository;

    public ProjectsController(IRepositoryBase<Employee> employeeRepository, IRepositoryBase<Project> projectRepository)
    {
        _employeeRepository = employeeRepository;
        _projectRepository = projectRepository;
    }
    [HttpGet]
    public ActionResult<IQueryable<Project>> GetProjects()
    {
        var result = _projectRepository.Get(null);
        return Ok(result);
    }
    [HttpGet("Search")]
    public ActionResult<IQueryable<Project>> SearchProjectByName(string name)
    {
        if (string.IsNullOrEmpty(name)) return ValidationProblem();
        var result = _projectRepository.Get(x => x.ProjectName.Contains(name));
        return Ok(result);
    }
    [HttpGet("InProgress")]
    public ActionResult<IQueryable<Project>> SearchProjectInProgress()
    {
        var result = _projectRepository.Get(x => x.ProjectEndDate == null || x.ProjectEndDate > DateTime.Now);
        return Ok(result);
    }
    [HttpGet("Finished")]
    public ActionResult<IQueryable<Project>> SearchProjectFinished()
    {
        var result = _projectRepository.Get(x => x.ProjectEndDate != null && x.ProjectEndDate < DateTime.Now);
        return Ok(result);
    }

    [HttpGet("{projectId:int}")]
    public async Task<ActionResult<Project?>> GetProjectDetails([FromRoute] int projectId, CancellationToken _)
    {
        var project = await _projectRepository.GetById(projectId, _);
        return project;
    }

    [HttpPost]
    public async Task<ActionResult<Project?>> CreateProject(CreateProjectRequest request, CancellationToken _)
    {
        if (request.ProjectName.Length < 2 || request.ProjectName.Length > 150)
            return ValidationProblem("Project Name length must be between 2 and 150");
        if (request.ProjectEndDate is not null && request.ProjectStartDate >= request.ProjectEndDate)
            return ValidationProblem("ProjectStartDate must < ProjectEndDate when ProjectEndDate is not NULL");

        var employees = await _employeeRepository.Get(x => request.EmployeeIds.Contains(x.EmployeeId))
            .Select(x => new ProjectEmployee { Employee = x, EmployeeId = x.EmployeeId }).ToListAsync(_);
        var project = new Project
        {
            ProjectName = request.ProjectName,
            ProjectEndDate = request.ProjectEndDate,
            ProjectStartDate = request.ProjectStartDate,
            ProjectEmployees = employees,
        };
        var res = await _projectRepository.Create(project, _);
        return Ok(res);
    }

    [HttpPut("{projectId:int}")]
    public async Task<ActionResult<Project?>> UpdateProject([FromRoute] int projectId, CreateProjectRequest request,
        CancellationToken _)
    {
        if (request.ProjectName.Length < 2 || request.ProjectName.Length > 150)
            return ValidationProblem("Project Name length must be between 2 and 150");
        if (request.ProjectEndDate is not null && request.ProjectStartDate >= request.ProjectEndDate)
            return ValidationProblem("ProjectStartDate must < ProjectEndDate when ProjectEndDate is not NULL");

        var employees = await _employeeRepository.Get(x => request.EmployeeIds.Contains(x.EmployeeId))
            .Select(x => new ProjectEmployee { Employee = x, EmployeeId = x.EmployeeId }).ToListAsync(_);
        var project = await _projectRepository.GetById(projectId, _);
        if (project is null) return NotFound("Project not found.");
        project.ProjectName = request.ProjectName;
        project.ProjectEndDate = request.ProjectEndDate;
        project.ProjectStartDate = request.ProjectStartDate;
        project.ProjectEmployees = employees;

        var res = await _projectRepository.Update(project, _);
        return Ok(res);
    }

    [HttpDelete("{projectId:int}")]
    public async Task<ActionResult<Project?>> Project([FromRoute] int projectId, CancellationToken _)
    {
        var res = await _projectRepository.Delete(projectId, _);
        return Ok(res);
    }
}