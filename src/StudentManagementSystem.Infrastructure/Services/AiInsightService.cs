using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.AI;
using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Shared.Exceptions;

namespace StudentManagementSystem.Infrastructure.Services;

public class AiInsightService : IAiInsightService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Kernel _kernel;
    private readonly ILogger<AiInsightService> _logger;

    private const double LowRiskThreshold = 85.0;
    private const double MediumRiskThreshold = 75.0;

    public AiInsightService(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<AiInsightService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;

        var apiKey = configuration["AiSettings:ApiKey"]
            ?? throw new InvalidOperationException("AiSettings:ApiKey is not configured.");
        var model = configuration["AiSettings:Model"] ?? "gpt-4o-mini";

        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(model, apiKey);
        _kernel = builder.Build();
    }

    public async Task<AttendanceRiskAnalysisDto> AnalyzeAttendanceRiskAsync(int studentId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId)
            ?? throw new NotFoundException("Student", studentId);

        var records = await _unitOfWork.Attendances.GetByStudentAsync(studentId);
        var total = records.Count;
        var present = records.Count(r => r.Status == AttendanceStatus.Present);
        var percentage = total == 0 ? 0 : Math.Round((double)present / total * 100, 2);

        var riskLevel = percentage >= LowRiskThreshold ? "Low"
            : percentage >= MediumRiskThreshold ? "Medium"
            : "High";

        var prompt = $"""
            You are an academic advisor assistant. Write a brief, 2-3 sentence summary
            for a teacher about this student's attendance record. Be factual and constructive,
            not alarmist. Do not invent any numbers beyond what is given.

            Student name: {student.FullName}
            Total classes recorded: {total}
            Classes present: {present}
            Attendance percentage: {percentage}%
            Risk level (already determined by the school's policy, not by you): {riskLevel}
            """;

        var aiSummary = await GetAiResponseAsync(prompt);

        return new AttendanceRiskAnalysisDto
        {
            StudentId = studentId,
            StudentName = student.FullName,
            AttendancePercentage = percentage,
            RiskLevel = riskLevel,
            AiSummary = aiSummary
        };
    }

    public async Task<PerformanceSummaryDto> GeneratePerformanceSummaryAsync(int studentId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId)
            ?? throw new NotFoundException("Student", studentId);

        var marksRecords = await _unitOfWork.MarksRecords.GetByStudentAsync(studentId);

        if (!marksRecords.Any())
        {
            return new PerformanceSummaryDto
            {
                StudentId = studentId,
                StudentName = student.FullName,
                OverallAveragePercentage = 0,
                AiSummary = "No marks have been recorded for this student yet."
            };
        }

        var overallAverage = Math.Round(
            marksRecords.Average(m => (m.ObtainedMarks / m.MaxMarks) * 100), 2);

        var subjectBreakdown = marksRecords
            .GroupBy(m => m.Subject.Name)
            .Select(g => $"{g.Key}: {Math.Round(g.Average(m => (m.ObtainedMarks / m.MaxMarks) * 100), 1)}%")
            .ToList();

        var prompt = $"""
            You are an academic advisor assistant. Write a brief, 3-4 sentence summary
            for a teacher about this student's exam performance across subjects.
            Highlight their strongest and weakest subject specifically. Be constructive.
            Do not invent any numbers beyond what is given.

            Student name: {student.FullName}
            Overall average: {overallAverage}%
            Subject breakdown:
            {string.Join("\n", subjectBreakdown)}
            """;

        var aiSummary = await GetAiResponseAsync(prompt);

        return new PerformanceSummaryDto
        {
            StudentId = studentId,
            StudentName = student.FullName,
            OverallAveragePercentage = overallAverage,
            AiSummary = aiSummary
        };
    }

    private async Task<string> GetAiResponseAsync(string prompt)
    {
        try
        {
            var chatService = _kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();
            history.AddUserMessage(prompt);

            var response = await chatService.GetChatMessageContentAsync(history, kernel: _kernel);
            return response.Content ?? "Unable to generate a summary at this time.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI insight generation failed.");
            return "AI summary is temporarily unavailable. Please review the raw data above.";
        }
    }
}