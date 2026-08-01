namespace StudentManagementSystem.Application.DTOs.Chat;

public class ConversationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<ChatMessageDto> Messages { get; set; } = new();
}