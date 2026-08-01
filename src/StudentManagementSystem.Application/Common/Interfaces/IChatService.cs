using StudentManagementSystem.Application.DTOs.Chat;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IChatService
{
    Task<ConversationDto> SendMessageAsync(int userId, SendMessageDto dto);
}