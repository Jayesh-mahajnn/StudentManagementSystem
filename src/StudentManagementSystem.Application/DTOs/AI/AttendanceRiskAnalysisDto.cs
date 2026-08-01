namespace StudentManagementSystem.Application.DTOs.AI;

public class AttendanceRiskAnalysisDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public double AttendancePercentage { get; set; }
    public string RiskLevel { get; set; } = string.Empty; // "Low" | "Medium" | "High" — computed by US, not the AI
    public string AiSummary { get; set; } = string.Empty; // the AI's natural-language narrative
}