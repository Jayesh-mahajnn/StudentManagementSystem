using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.AI;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Teacher")]
public class AiInsightsController : ControllerBase
{
    private readonly IAiInsightService _aiInsightService;

    public AiInsightsController(IAiInsightService aiInsightService)
    {
        _aiInsightService = aiInsightService;
    }

    [HttpGet("attendance-risk/{studentId:int}")]
    public async Task<ActionResult<AttendanceRiskAnalysisDto>> GetAttendanceRisk(int studentId)
    {
        return Ok(await _aiInsightService.AnalyzeAttendanceRiskAsync(studentId));
    }

    [HttpGet("performance-summary/{studentId:int}")]
    public async Task<ActionResult<PerformanceSummaryDto>> GetPerformanceSummary(int studentId)
    {
        return Ok(await _aiInsightService.GeneratePerformanceSummaryAsync(studentId));
    }
}