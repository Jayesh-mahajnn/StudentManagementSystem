using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Dashboard;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<ActionResult<AdminDashboardDto>> GetAdminDashboard()
    {
        return Ok(await _dashboardService.GetAdminDashboardAsync());
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet("teacher")]
    public async Task<ActionResult<TeacherDashboardDto>> GetTeacherDashboard()
    {
        var teacherId = GetLinkedProfileId("TeacherId");
        return Ok(await _dashboardService.GetTeacherDashboardAsync(teacherId));
    }

    [Authorize(Roles = "Student")]
    [HttpGet("student")]
    public async Task<ActionResult<StudentDashboardDto>> GetStudentDashboard()
    {
        var studentId = GetLinkedProfileId("StudentId");
        return Ok(await _dashboardService.GetStudentDashboardAsync(studentId));
    }

    private int GetLinkedProfileId(string claimType)
    {
        var value = User.FindFirst(claimType)?.Value;
        if (string.IsNullOrEmpty(value))
            throw new UnauthorizedAccessException($"This account has no linked {claimType} — contact an administrator.");
        return int.Parse(value);
    }
}