using StudentManagementSystem.Application.DTOs.AI;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IAiInsightService
{
    Task<AttendanceRiskAnalysisDto> AnalyzeAttendanceRiskAsync(int studentId);
    Task<PerformanceSummaryDto> GeneratePerformanceSummaryAsync(int studentId);
}