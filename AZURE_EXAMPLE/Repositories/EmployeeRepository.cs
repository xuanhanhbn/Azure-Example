using System.Linq.Expressions;
using AZURE_EXAMPLE.Models;
using Microsoft.EntityFrameworkCore;

namespace AZURE_EXAMPLE.Repositories;

public class EmployeeRepository : IRepositoryBase<Employee>
{
    private readonly AppDbContext _dbContext;

    public EmployeeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Employee> Get(Expression<Func<Employee, bool>>? expression, int size = 10, int page = 0)
    {
        return _dbContext.Employees.Where(expression ?? (x => true)).Take(size).Skip(size * page);
    }

    public async Task<Employee?> GetById(int id, CancellationToken _ = default)
    {
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id, _);
        return employee;
    }

    public async Task<Employee?> Create(Employee entity, CancellationToken _ = default)
    {
        var entry = _dbContext.Employees.Add(entity);
        var res = await _dbContext.SaveChangesAsync(_);
        if (res > 0)
        {
            return entry.Entity;
        }

        return null;
    }

    public async Task<Employee?> Update(Employee entity, CancellationToken _ = default)
    {
        var entry = _dbContext.Employees.Update(entity);
        var res = await _dbContext.SaveChangesAsync(_);
        if (res > 0)
        {
            return entry.Entity;
        }

        return null;
    }

    public async Task<bool> Delete(int id, CancellationToken _ = default)
    {
        var employee = await GetById(id, _);
        if (employee is null) return false;
        _dbContext.Employees.Remove(employee);
        var res = await _dbContext.SaveChangesAsync(_);
        return res > 0;

    }
}