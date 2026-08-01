using StudentManagementSystem.Domain.Common;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Entities;

public class ChatMessage : BaseEntity
{
    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;

    public ChatRole Role { get; set; } // User or Assistant
    public string Content { get; set; } = string.Empty;
}