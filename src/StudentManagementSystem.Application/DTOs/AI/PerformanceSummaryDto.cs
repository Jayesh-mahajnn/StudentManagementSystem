namespace StudentManagementSystem.Application.DTOs.AI;

public class PerformanceSummaryDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public decimal OverallAveragePercentage { get; set; }
    public string AiSummary { get; set; } = string.Empty;
}