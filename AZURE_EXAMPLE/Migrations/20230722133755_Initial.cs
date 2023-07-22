using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZURE_EXAMPLE.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Employees",
            columns: table => new
            {
                EmployeeId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                EmployeeDOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                EmployeeDepartment = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Employees", x => x.EmployeeId);
            });

        migrationBuilder.CreateTable(
            name: "Projects",
            columns: table => new
            {
                ProjectId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ProjectStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                ProjectEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Projects", x => x.ProjectId);
            });

        migrationBuilder.CreateTable(
            name: "ProjectEmployee",
            columns: table => new
            {
                ProjectEmployeeId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                EmployeeId = table.Column<int>(type: "int", nullable: false),
                ProjectId = table.Column<int>(type: "int", nullable: false),
                Tasks = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProjectEmployee", x => x.ProjectEmployeeId);
                table.ForeignKey(
                    name: "FK_ProjectEmployee_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "EmployeeId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ProjectEmployee_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "ProjectId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ProjectEmployee_EmployeeId",
            table: "ProjectEmployee",
            column: "EmployeeId");

        migrationBuilder.CreateIndex(
            name: "IX_ProjectEmployee_ProjectId",
            table: "ProjectEmployee",
            column: "ProjectId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ProjectEmployee");

        migrationBuilder.DropTable(
            name: "Employees");

        migrationBuilder.DropTable(
            name: "Projects");
    }
}
