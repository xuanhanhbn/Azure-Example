using System.Linq.Expressions;
using AZURE_EXAMPLE.Models;
using Microsoft.EntityFrameworkCore;

namespace AZURE_EXAMPLE.Repositories;

public class ProjectRepository : IRepositoryBase<Project>
{
    private readonly AppDbContext _dbContext;

    public ProjectRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IQueryable<Project> Get(Expression<Func<Project, bool>>? expression, int size = 10, int page = 0)
    {
        return _dbContext.Projects.Where(expression ?? (x => true)).Take(size).Skip(size * page);
    }

    public async Task<Project?> GetById(int id, CancellationToken _ = default)
    {
        var project = await _dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == id, _);
        return project;
    }

    public async Task<Project?> Create(Project entity, CancellationToken _ = default)
    {
        var entry = _dbContext.Projects.Add(entity);
        var res = await _dbContext.SaveChangesAsync(_);
        if (res > 0)
        {
            return entry.Entity;
        }

        return null;
    }

    public async Task<Project?> Update(Project entity, CancellationToken _ = default)
    {
        var entry = _dbContext.Projects.Update(entity);
        var res = await _dbContext.SaveChangesAsync(_);
        if (res > 0)
        {
            return entry.Entity;
        }

        return null;
    }

    public async Task<bool> Delete(int id, CancellationToken _ = default)
    {
        var project = await GetById(id, _);
        if (project is null) return false;
        _dbContext.Projects.Remove(project);
        var res = await _dbContext.SaveChangesAsync(_);
        return res > 0;

    }
}