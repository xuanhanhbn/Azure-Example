namespace AZURE_EXAMPLE.Dto;

public class CreateEmployeeRequest
{
    public string EmployeeName { get; set; }
    public DateTime EmployeeDOB { get; set; }
    public string EmployeeDepartment { get; set; }
}