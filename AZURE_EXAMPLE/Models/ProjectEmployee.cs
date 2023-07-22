using System.ComponentModel.DataAnnotations.Schema;

namespace AZURE_EXAMPLE.Models;

public class ProjectEmployee
{
    public int ProjectEmployeeId { get; set; }
    public int EmployeeId { get; set; }
    public virtual Employee Employee { get; set; }
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; }
    public string Tasks { get; set; }
}