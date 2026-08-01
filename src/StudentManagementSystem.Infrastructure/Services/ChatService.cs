using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs.Chat;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Kernel _kernel;
    private readonly ILogger<ChatService> _logger;
    private const int MaxHistoryMessages = 20; // pragmatic truncation limit

    public ChatService(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<ChatService> logger)
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

    public async Task<ConversationDto> SendMessageAsync(int userId, SendMessageDto dto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId)
            ?? throw new StudentManagementSystem.Shared.Exceptions.NotFoundException("User", userId);

        // Get or create the conversation
        Conversation? conversation = dto.ConversationId.HasValue
            ? await _unitOfWork.Conversations.GetWithMessagesAsync(dto.ConversationId.Value, userId)
            : null;

        if (conversation is null)
        {
            conversation = new Conversation
            {
                UserId = userId,
                Title = dto.Message.Length > 50 ? dto.Message[..50] + "..." : dto.Message
            };
            await _unitOfWork.Conversations.AddAsync(conversation);
            await _unitOfWork.SaveChangesAsync(); // need the generated Id before adding messages
        }

        // Save the user's new message
        var userMessage = new ChatMessage
        {
            ConversationId = conversation.Id,
            Role = ChatRole.User,
            Content = dto.Message
        };
        conversation.Messages.Add(userMessage);
        await _unitOfWork.SaveChangesAsync();

        // Build context: who is this user, and (if Student) a summary of their own data
        var contextPrompt = await BuildContextPromptAsync(user);

        // Build the full chat history to send to the AI (system + truncated history)
        var history = new ChatHistory(contextPrompt);
        var recentMessages = conversation.Messages
            .OrderBy(m => m.CreatedAt)
            .TakeLast(MaxHistoryMessages);

        foreach (var msg in recentMessages)
        {
            if (msg.Role == ChatRole.User) history.AddUserMessage(msg.Content);
            else history.AddAssistantMessage(msg.Content);
        }

        string assistantReply;
        try
        {
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            var response = await chatCompletionService.GetChatMessageContentAsync(history, kernel: _kernel);
            assistantReply = response.Content ?? "I couldn't generate a response. Please try again.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Chat AI response failed.");
            assistantReply = "I'm having trouble responding right now. Please try again in a moment.";
        }

        // Save the assistant's reply
        var assistantMessage = new ChatMessage
        {
            ConversationId = conversation.Id,
            Role = ChatRole.Assistant,
            Content = assistantReply
        };
        conversation.Messages.Add(assistantMessage);
        await _unitOfWork.SaveChangesAsync();

        return new ConversationDto
        {
            Id = conversation.Id,
            Title = conversation.Title,
            Messages = conversation.Messages
                .OrderBy(m => m.CreatedAt)
                .Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    Role = m.Role.ToString(),
                    Content = m.Content,
                    CreatedAt = m.CreatedAt
                }).ToList()
        };
    }

    private async Task<string> BuildContextPromptAsync(User user)
    {
        var basePrompt = "You are a helpful academic assistant for a Student Management System. " +
                          "Answer questions clearly and concisely. Only use the context data given below; " +
                          "do not invent information you were not given.";

        if (user.Role == UserRole.Student && user.StudentId.HasValue)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(user.StudentId.Value);
            var attendanceRecords = await _unitOfWork.Attendances.GetByStudentAsync(user.StudentId.Value);
            var marksRecords = await _unitOfWork.MarksRecords.GetByStudentAsync(user.StudentId.Value);

            var totalClasses = attendanceRecords.Count;
            var present = attendanceRecords.Count(a => a.Status == AttendanceStatus.Present);
            var attendancePct = totalClasses == 0 ? 0 : Math.Round((double)present / totalClasses * 100, 2);

            var avgMarks = marksRecords.Any()
                ? Math.Round(marksRecords.Average(m => (m.ObtainedMarks / m.MaxMarks) * 100), 2)
                : 0;

            basePrompt += $"""

                You are speaking with the student: {student?.FullName}.
                Their attendance percentage is: {attendancePct}%.
                Their overall marks average is: {avgMarks}%.
                Only discuss this student's own data. Do not discuss or speculate about any other student.
                """;
        }

        return basePrompt;
    }
}