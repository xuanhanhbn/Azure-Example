namespace AZURE_EXAMPLE.Dto;

public class CreateProjectRequest
{
    public string ProjectName { get; set; }
    public DateTime ProjectStartDate { get; set; }
    public DateTime? ProjectEndDate { get; set; }
    public List<int> EmployeeIds { get; set; }
}