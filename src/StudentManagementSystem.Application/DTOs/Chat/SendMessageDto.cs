namespace StudentManagementSystem.Application.DTOs.Chat;

public class SendMessageDto
{
    public int? ConversationId { get; set; } // null = start a new conversation
    public string Message { get; set; } = string.Empty;
}